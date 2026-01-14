import { useState, useEffect } from 'react'
import { Container, Table, Button, Modal, Form, Pagination, Alert } from 'react-bootstrap'
import { userService } from '../services/api'
import { toast } from 'react-toastify'

const Users = () => {
  const [users, setUsers] = useState([])
  const [showModal, setShowModal] = useState(false)
  const [editingUser, setEditingUser] = useState(null)
  const [loading, setLoading] = useState(false)
  const [currentPage, setCurrentPage] = useState(1)
  const [pageSize] = useState(10)
  const [totalPages, setTotalPages] = useState(1)
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: '',
    role: 'Cashier'
  })

  useEffect(() => {
    loadUsers()
  }, [currentPage])

  const loadUsers = async () => {
    try {
      const response = await userService.getAll(currentPage, pageSize)
      setUsers(response.data.items || [])
      setTotalPages(response.data.totalPages || 1)
    } catch (error) {
      toast.error('Failed to load users')
    }
  }

  const handleShow = (user = null) => {
    if (user) {
      setEditingUser(user)
      setFormData({
        name: user.name,
        email: user.email,
        password: '',
        role: user.role
      })
    } else {
      setEditingUser(null)
      setFormData({
        name: '',
        email: '',
        password: '',
        role: 'Cashier'
      })
    }
    setShowModal(true)
  }

  const handleClose = () => {
    setShowModal(false)
    setEditingUser(null)
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)

    try {
      if (editingUser) {
        await userService.update(editingUser.id, formData)
        toast.success('User updated successfully')
      } else {
        await userService.create(formData)
        toast.success('User created successfully')
      }
      handleClose()
      loadUsers()
    } catch (error) {
      const errorMessage = error.response?.data?.message || error.response?.data || error.message || 'Operation failed'
      toast.error(errorMessage)
      console.error('Error:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this user?')) {
      try {
        await userService.delete(id)
        toast.success('User deleted successfully')
        loadUsers()
      } catch (error) {
        const errorMessage = error.response?.data?.message || error.response?.data || error.message || 'Failed to delete user'
        toast.error(errorMessage)
        console.error('Delete error:', error)
      }
    }
  }

  return (
    <Container>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>User Management</h1>
        <Button variant="primary" onClick={() => handleShow()}>
          Add User
        </Button>
      </div>

      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Role</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {users.map((user) => (
            <tr key={user.id}>
              <td>{user.name}</td>
              <td>{user.email}</td>
              <td>{user.role}</td>
              <td>
                <Button
                  variant="warning"
                  size="sm"
                  className="me-2"
                  onClick={() => handleShow(user)}
                >
                  Edit
                </Button>
                <Button
                  variant="danger"
                  size="sm"
                  onClick={() => handleDelete(user.id)}
                >
                  Delete
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      <Pagination>
        <Pagination.Prev
          disabled={currentPage === 1}
          onClick={() => setCurrentPage(currentPage - 1)}
        />
        <Pagination.Item active>{currentPage}</Pagination.Item>
        <Pagination.Next
          disabled={currentPage >= totalPages}
          onClick={() => setCurrentPage(currentPage + 1)}
        />
      </Pagination>

      <Modal show={showModal} onHide={handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>{editingUser ? 'Edit User' : 'Add User'}</Modal.Title>
        </Modal.Header>
        <Form onSubmit={handleSubmit}>
          <Modal.Body>
            <Form.Group className="mb-3">
              <Form.Label>Name</Form.Label>
              <Form.Control
                type="text"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                required
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Email</Form.Label>
              <Form.Control
                type="email"
                value={formData.email}
                onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                required
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Password {editingUser && <small className="text-muted">(required to update)</small>}</Form.Label>
              <Form.Control
                type="password"
                value={formData.password}
                onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                required
                placeholder={editingUser ? 'Enter new password' : 'Enter password'}
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Role</Form.Label>
              <Form.Select
                value={formData.role}
                onChange={(e) => setFormData({ ...formData, role: e.target.value })}
              >
                <option value="Admin">Admin</option>
                <option value="Cashier">Cashier</option>
              </Form.Select>
            </Form.Group>
          </Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={handleClose}>
              Cancel
            </Button>
            <Button variant="primary" type="submit" disabled={loading}>
              {loading ? 'Saving...' : 'Save'}
            </Button>
          </Modal.Footer>
        </Form>
      </Modal>
    </Container>
  )
}

export default Users

