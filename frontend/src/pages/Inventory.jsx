import { useState, useEffect } from 'react'
import { Container, Table, Button, Modal, Form, Pagination, Alert } from 'react-bootstrap'
import { inventoryService } from '../services/api'
import { toast } from 'react-toastify'

const Inventory = () => {
  const [inventory, setInventory] = useState([])
  const [showModal, setShowModal] = useState(false)
  const [editingItem, setEditingItem] = useState(null)
  const [loading, setLoading] = useState(false)
  const [currentPage, setCurrentPage] = useState(1)
  const [pageSize] = useState(10)
  const [totalPages, setTotalPages] = useState(1)
  const [formData, setFormData] = useState({
    itemName: '',
    unit: '',
    quantityAvailable: '',
    reorderLevel: ''
  })

  useEffect(() => {
    loadInventory()
  }, [currentPage])

  const loadInventory = async () => {
    try {
      const response = await inventoryService.getAll(currentPage, pageSize)
      setInventory(response.data.items || [])
      setTotalPages(response.data.totalPages || 1)
    } catch (error) {
      toast.error('Failed to load inventory')
    }
  }

  const handleShow = (item = null) => {
    if (item) {
      setEditingItem(item)
      setFormData({
        itemName: item.itemName,
        unit: item.unit,
        quantityAvailable: item.quantityAvailable,
        reorderLevel: item.reorderLevel
      })
    } else {
      setEditingItem(null)
      setFormData({
        itemName: '',
        unit: '',
        quantityAvailable: '',
        reorderLevel: ''
      })
    }
    setShowModal(true)
  }

  const handleClose = () => {
    setShowModal(false)
    setEditingItem(null)
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)

    try {
      const submitData = {
        ...formData,
        quantityAvailable: parseFloat(formData.quantityAvailable),
        reorderLevel: parseFloat(formData.reorderLevel)
      }
      if (editingItem) {
        await inventoryService.update(editingItem.id, submitData)
        toast.success('Inventory updated successfully')
      } else {
        await inventoryService.create(submitData)
        toast.success('Inventory item created successfully')
      }
      handleClose()
      loadInventory()
    } catch (error) {
      const errorMessage = error.response?.data?.message || error.response?.data || error.message || 'Operation failed'
      toast.error(errorMessage)
      console.error('Error:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this inventory item?')) {
      try {
        await inventoryService.delete(id)
        toast.success('Inventory item deleted successfully')
        loadInventory()
      } catch (error) {
        const errorMessage = error.response?.data?.message || error.response?.data || error.message || 'Failed to delete inventory item'
        toast.error(errorMessage)
        console.error('Delete error:', error)
      }
    }
  }

  return (
    <Container>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>Inventory Management</h1>
        <Button variant="primary" onClick={() => handleShow()}>
          Add Inventory Item
        </Button>
      </div>

      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Item Name</th>
            <th>Unit</th>
            <th>Quantity Available</th>
            <th>Reorder Level</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {inventory.map((item) => (
            <tr key={item.id}>
              <td>{item.itemName}</td>
              <td>{item.unit}</td>
              <td>{item.quantityAvailable}</td>
              <td>{item.reorderLevel}</td>
              <td>
                <Button
                  variant="warning"
                  size="sm"
                  className="me-2"
                  onClick={() => handleShow(item)}
                >
                  Edit
                </Button>
                <Button
                  variant="danger"
                  size="sm"
                  onClick={() => handleDelete(item.id)}
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
          <Modal.Title>{editingItem ? 'Edit Inventory Item' : 'Add Inventory Item'}</Modal.Title>
        </Modal.Header>
        <Form onSubmit={handleSubmit}>
          <Modal.Body>
            <Form.Group className="mb-3">
              <Form.Label>Item Name</Form.Label>
              <Form.Control
                type="text"
                value={formData.itemName}
                onChange={(e) => setFormData({ ...formData, itemName: e.target.value })}
                required
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Unit</Form.Label>
              <Form.Control
                type="text"
                value={formData.unit}
                onChange={(e) => setFormData({ ...formData, unit: e.target.value })}
                placeholder="e.g., kg, liters, pieces"
                required
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Quantity Available</Form.Label>
              <Form.Control
                type="number"
                step="0.01"
                value={formData.quantityAvailable}
                onChange={(e) => setFormData({ ...formData, quantityAvailable: e.target.value })}
                required
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Reorder Level</Form.Label>
              <Form.Control
                type="number"
                step="0.01"
                value={formData.reorderLevel}
                onChange={(e) => setFormData({ ...formData, reorderLevel: e.target.value })}
                required
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
    </Container>
  )
}

export default Inventory

