import { useState, useEffect } from 'react'
import { Container, Table, Button, Modal, Form, Pagination, Badge } from 'react-bootstrap'
import { menuService } from '../services/api'
import { toast } from 'react-toastify'
import { useAuth } from '../contexts/AuthContext'

const Menus = () => {
  const { isAdmin } = useAuth()
  const [menus, setMenus] = useState([])
  const [showModal, setShowModal] = useState(false)
  const [editingMenu, setEditingMenu] = useState(null)
  const [loading, setLoading] = useState(false)
  const [currentPage, setCurrentPage] = useState(1)
  const [pageSize] = useState(10)
  const [totalPages, setTotalPages] = useState(1)
  const [formData, setFormData] = useState({
    menuName: '',
    price: '',
    description: '',
    isAvailable: true
  })

  useEffect(() => {
    loadMenus()
  }, [currentPage])

  const loadMenus = async () => {
    try {
      const response = await menuService.getAll(currentPage, pageSize)
      setMenus(response.data.items || [])
      setTotalPages(response.data.totalPages || 1)
    } catch (error) {
      toast.error('Failed to load menus')
    }
  }

  const handleShow = (menu = null) => {
    if (menu) {
      setEditingMenu(menu)
      setFormData({
        menuName: menu.menuName,
        price: menu.price,
        description: menu.description || '',
        isAvailable: menu.isAvailable
      })
    } else {
      setEditingMenu(null)
      setFormData({
        menuName: '',
        price: '',
        description: '',
        isAvailable: true
      })
    }
    setShowModal(true)
  }

  const handleClose = () => {
    setShowModal(false)
    setEditingMenu(null)
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)

    try {
      const submitData = {
        ...formData,
        price: parseFloat(formData.price)
      }
      if (editingMenu) {
        const menuId = editingMenu.menuId || editingMenu.id
        await menuService.update(menuId, submitData)
        toast.success('Menu updated successfully')
      } else {
        await menuService.create(submitData)
        toast.success('Menu created successfully')
      }
      handleClose()
      loadMenus()
    } catch (error) {
      const errorMessage = error.response?.data?.message || error.response?.data || error.message || 'Operation failed'
      toast.error(errorMessage)
      console.error('Error:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleDelete = async (menu) => {
    if (window.confirm('Are you sure you want to delete this menu?')) {
      try {
        const menuId = menu.menuId || menu.id
        await menuService.delete(menuId)
        toast.success('Menu deleted successfully')
        loadMenus()
      } catch (error) {
        const errorMessage = error.response?.data?.message || error.response?.data || error.message || 'Failed to delete menu'
        toast.error(errorMessage)
        console.error('Delete error:', error)
      }
    }
  }

  return (
    <Container>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>Menu Management</h1>
        {isAdmin() && (
          <Button variant="primary" onClick={() => handleShow()}>
            Add Menu
          </Button>
        )}
      </div>

      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Menu Name</th>
            <th>Price</th>
            <th>Description</th>
            <th>Status</th>
            {isAdmin() && <th>Actions</th>}
          </tr>
        </thead>
        <tbody>
          {menus.map((menu) => (
            <tr key={menu.menuId || menu.id}>
              <td>{menu.menuName}</td>
              <td>${menu.price}</td>
              <td>{menu.description || '-'}</td>
              <td>
                <Badge bg={menu.isAvailable ? 'success' : 'danger'}>
                  {menu.isAvailable ? 'Available' : 'Unavailable'}
                </Badge>
              </td>
              {isAdmin() && (
                <td>
                  <Button
                    variant="warning"
                    size="sm"
                    className="me-2"
                    onClick={() => handleShow(menu)}
                  >
                    Edit
                  </Button>
                  <Button
                    variant="danger"
                    size="sm"
                    onClick={() => handleDelete(menu)}
                  >
                    Delete
                  </Button>
                </td>
              )}
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

      {isAdmin() && (
        <Modal show={showModal} onHide={handleClose}>
          <Modal.Header closeButton>
            <Modal.Title>{editingMenu ? 'Edit Menu' : 'Add Menu'}</Modal.Title>
          </Modal.Header>
          <Form onSubmit={handleSubmit}>
            <Modal.Body>
              <Form.Group className="mb-3">
                <Form.Label>Menu Name</Form.Label>
                <Form.Control
                  type="text"
                  value={formData.menuName}
                  onChange={(e) => setFormData({ ...formData, menuName: e.target.value })}
                  required
                />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Price</Form.Label>
                <Form.Control
                  type="number"
                  step="0.01"
                  value={formData.price}
                  onChange={(e) => setFormData({ ...formData, price: e.target.value })}
                  required
                />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Description</Form.Label>
                <Form.Control
                  as="textarea"
                  rows={3}
                  value={formData.description}
                  onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Check
                  type="checkbox"
                  label="Available"
                  checked={formData.isAvailable}
                  onChange={(e) => setFormData({ ...formData, isAvailable: e.target.checked })}
                />
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
      )}
    </Container>
  )
}

export default Menus

