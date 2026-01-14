import { useState, useEffect } from 'react'
import { Container, Table, Button, Modal, Form, Pagination, Badge, Row, Col, Alert } from 'react-bootstrap'
import { orderService, menuService } from '../services/api'
import { toast } from 'react-toastify'
import { useAuth } from '../contexts/AuthContext'

const Orders = () => {
  const { isCashier, isAdmin } = useAuth()
  const [orders, setOrders] = useState([])
  const [showModal, setShowModal] = useState(false)
  const [showViewModal, setShowViewModal] = useState(false)
  const [showShortageModal, setShowShortageModal] = useState(false)
  const [shortageItems, setShortageItems] = useState([])
  const [selectedOrder, setSelectedOrder] = useState(null)
  const [menus, setMenus] = useState([])
  const [loading, setLoading] = useState(false)
  const [currentPage, setCurrentPage] = useState(1)
  const [pageSize] = useState(10)
  const [totalPages, setTotalPages] = useState(1)
  const [formData, setFormData] = useState({
    customerName: '',
    customerEmail: '',
    address: '',
    orderItems: [{ menuId: '', quantityOrdered: 1 }]
  })

  useEffect(() => {
    loadOrders()
    loadMenus() // Always load menus to display menu names in view
  }, [currentPage])

  const loadOrders = async () => {
    try {
      const response = await orderService.getAll(currentPage, pageSize)
      setOrders(response.data.items || [])
      setTotalPages(response.data.totalPages || 1)
    } catch (error) {
      toast.error('Failed to load orders')
    }
  }

  const loadMenus = async () => {
    try {
      const response = await menuService.getAll(1, 100)
      setMenus(response.data.items || [])
    } catch (error) {
      toast.error('Failed to load menus')
    }
  }

  const handleShow = () => {
    setSelectedOrder(null)
    setFormData({
      customerName: '',
      customerEmail: '',
      address: '',
      orderItems: [{ menuId: '', quantityOrdered: 1 }]
    })
    setShowModal(true)
  }

  const handleView = (order) => {
    setSelectedOrder(order)
    setShowViewModal(true)
  }

  const handleClose = () => {
    setShowModal(false)
    setShowViewModal(false)
    setShowShortageModal(false)
    setShortageItems([])
    setSelectedOrder(null)
  }

  const handleAddOrderItem = () => {
    setFormData({
      ...formData,
      orderItems: [...formData.orderItems, { menuId: '', quantityOrdered: 1 }]
    })
  }

  const handleRemoveOrderItem = (index) => {
    const newItems = formData.orderItems.filter((_, i) => i !== index)
    setFormData({ ...formData, orderItems: newItems })
  }

  const handleOrderItemChange = (index, field, value) => {
    const newItems = [...formData.orderItems]
    newItems[index][field] = field === 'quantityOrdered' ? parseInt(value) || 1 : value
    setFormData({ ...formData, orderItems: newItems })
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)

    try {
      const response = await orderService.create(formData)
      const result = response.data
      
      // Check if order was successful
      if (result.isSuccess || result.IsSuccess) {
        toast.success(result.message || result.Message || 'Order created successfully')
        handleClose()
        loadOrders()
      } else {
        // Check for shortage items
        const shortages = result.shortageItems || result.ShortageItems || []
        if (shortages.length > 0) {
          setShortageItems(shortages)
          setShowShortageModal(true)
          toast.warning(result.message || result.Message || 'Order cannot be completed due to inventory shortage')
        } else {
          toast.error(result.message || result.Message || 'Order creation failed')
        }
      }
    } catch (error) {
      const errorMessage = error.response?.data?.message || error.response?.data?.Message || error.message || 'Operation failed'
      toast.error(errorMessage)
      console.error('Error:', error)
      
      // Check if error response has shortage items
      if (error.response?.data?.shortageItems || error.response?.data?.ShortageItems) {
        const shortages = error.response.data.shortageItems || error.response.data.ShortageItems
        if (shortages.length > 0) {
          setShortageItems(shortages)
          setShowShortageModal(true)
        }
      }
    } finally {
      setLoading(false)
    }
  }

  const getMenuName = (menuId) => {
    const menu = menus.find(m => (m.menuId || m.id) === menuId)
    return menu ? menu.menuName : `Menu ID: ${menuId}`
  }

  const getMenuPrice = (menuId) => {
    const menu = menus.find(m => (m.menuId || m.id) === menuId)
    return menu ? menu.price : 0
  }

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this order?')) {
      try {
        await orderService.delete(id)
        toast.success('Order deleted successfully')
        loadOrders()
      } catch (error) {
        const errorMessage = error.response?.data?.message || error.response?.data || error.message || 'Failed to delete order'
        toast.error(errorMessage)
        console.error('Delete error:', error)
      }
    }
  }

  return (
    <Container>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>Order Management</h1>
        {(isCashier() || isAdmin()) && (
          <Button variant="primary" onClick={() => handleShow()}>
            Create Order
          </Button>
        )}
      </div>

      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Customer Name</th>
            <th>Customer Email</th>
            <th>Total Amount</th>
            <th>Date</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <tr key={order.id}>
              <td>{order.customerName}</td>
              <td>{order.customerEmail}</td>
              <td>${order.totalAmount?.toFixed(2) || '0.00'}</td>
              <td>{new Date(order.createdDate).toLocaleDateString()}</td>
              <td>
                <Button
                  variant="info"
                  size="sm"
                  className="me-2"
                  onClick={() => handleView(order)}
                >
                  View
                </Button>
                {(isCashier() || isAdmin()) && (
                  <Button
                    variant="danger"
                    size="sm"
                    onClick={() => handleDelete(order.id)}
                  >
                    Delete
                  </Button>
                )}
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

      {/* View Order Modal */}
      <Modal show={showViewModal} onHide={handleClose} size="lg">
        <Modal.Header closeButton>
          <Modal.Title>Order Details</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          {selectedOrder && (
            <>
              <Row className="mb-3">
                <Col><strong>Customer Name:</strong> {selectedOrder.customerName}</Col>
                <Col><strong>Email:</strong> {selectedOrder.customerEmail}</Col>
              </Row>
              <Row className="mb-3">
                <Col><strong>Address:</strong> {selectedOrder.address}</Col>
              </Row>
              <Row className="mb-3">
                <Col><strong>Total Amount:</strong> ${selectedOrder.totalAmount?.toFixed(2) || '0.00'}</Col>
              </Row>
              <h5 className="mb-3">Order Items:</h5>
              <Table striped bordered>
                <thead>
                  <tr>
                    <th>Product Name</th>
                    <th>Price</th>
                    <th>Quantity</th>
                    <th>Subtotal</th>
                  </tr>
                </thead>
                <tbody>
                  {selectedOrder.orderItems?.map((item, idx) => {
                    const menuId = item.menuId
                    const menuName = getMenuName(menuId)
                    const menuPrice = getMenuPrice(menuId)
                    const quantity = item.quantityOrdered || item.quantity || 0
                    const subtotal = menuPrice * quantity
                    
                    return (
                      <tr key={idx}>
                        <td>{menuName}</td>
                        <td>${menuPrice.toFixed(2)}</td>
                        <td>{quantity}</td>
                        <td>${subtotal.toFixed(2)}</td>
                      </tr>
                    )
                  })}
                </tbody>
                <tfoot>
                  <tr>
                    <td colSpan="3" className="text-end"><strong>Total:</strong></td>
                    <td><strong>${selectedOrder.totalAmount?.toFixed(2) || '0.00'}</strong></td>
                  </tr>
                </tfoot>
              </Table>
            </>
          )}
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>
            Close
          </Button>
        </Modal.Footer>
      </Modal>

      {/* Shortage Items Modal */}
      <Modal show={showShortageModal} onHide={handleClose} size="lg">
        <Modal.Header closeButton className="bg-warning text-dark">
          <Modal.Title>⚠️ Inventory Shortage</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Alert variant="warning" className="mb-3">
            <strong>Order cannot be completed!</strong> The following items are in short supply:
          </Alert>
          <Table striped bordered hover>
            <thead>
              <tr>
                <th>Menu Item</th>
                <th>Inventory Item</th>
                <th>Required</th>
                <th>Available</th>
                <th>Shortage</th>
              </tr>
            </thead>
            <tbody>
              {shortageItems.map((item, idx) => (
                <tr key={idx}>
                  <td>{item.menuName || item.MenuName || `Menu ID: ${item.menuId || item.MenuId}`}</td>
                  <td>{item.itemName || item.ItemName || `Item ID: ${item.itemId || item.ItemId}`}</td>
                  <td className="text-primary">
                    <strong>{item.requiredQuantity || item.RequiredQuantity || 0}</strong>
                  </td>
                  <td className="text-success">
                    <strong>{item.availableQuantity || item.AvailableQuantity || 0}</strong>
                  </td>
                  <td className="text-danger">
                    <strong>-{item.shortageQuantity || item.ShortageQuantity || 0}</strong>
                  </td>
                </tr>
              ))}
            </tbody>
          </Table>
          <Alert variant="info" className="mt-3 mb-0">
            <small>
              <strong>Note:</strong> Please update the inventory or reduce the order quantity to proceed.
            </small>
          </Alert>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>
            Close
          </Button>
        </Modal.Footer>
      </Modal>

      {/* Create Order Modal */}
      {(isCashier() || isAdmin()) && (
        <Modal show={showModal} onHide={handleClose} size="lg">
          <Modal.Header closeButton>
            <Modal.Title>Create Order</Modal.Title>
          </Modal.Header>
          <Form onSubmit={handleSubmit}>
            <Modal.Body>
              <Form.Group className="mb-3">
                <Form.Label>Customer Name</Form.Label>
                <Form.Control
                  type="text"
                  value={formData.customerName}
                  onChange={(e) => setFormData({ ...formData, customerName: e.target.value })}
                  required
                />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Customer Email</Form.Label>
                <Form.Control
                  type="email"
                  value={formData.customerEmail}
                  onChange={(e) => setFormData({ ...formData, customerEmail: e.target.value })}
                  required
                />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Address</Form.Label>
                <Form.Control
                  as="textarea"
                  rows={2}
                  value={formData.address}
                  onChange={(e) => setFormData({ ...formData, address: e.target.value })}
                  required
                />
              </Form.Group>
              <div className="d-flex justify-content-between align-items-center mb-3">
                <h5>Order Items</h5>
                <Button variant="success" size="sm" onClick={handleAddOrderItem}>
                  Add Item
                </Button>
              </div>
              {formData.orderItems.map((item, index) => (
                <Row key={index} className="mb-3">
                  <Col md={6}>
                    <Form.Select
                      value={item.menuId}
                      onChange={(e) => handleOrderItemChange(index, 'menuId', e.target.value)}
                      required
                    >
                      <option value="">Select Menu</option>
                      {menus.map((menu) => {
                        const menuIdValue = menu.menuId || menu.id
                        return (
                          <option key={menuIdValue} value={menuIdValue}>
                            {menu.menuName} - ${menu.price}
                          </option>
                        )
                      })}
                    </Form.Select>
                  </Col>
                  <Col md={4}>
                    <Form.Control
                      type="number"
                      min="1"
                      value={item.quantityOrdered}
                      onChange={(e) => handleOrderItemChange(index, 'quantityOrdered', e.target.value)}
                      required
                    />
                  </Col>
                  <Col md={2}>
                    <Button
                      variant="danger"
                      size="sm"
                      onClick={() => handleRemoveOrderItem(index)}
                      disabled={formData.orderItems.length === 1}
                    >
                      Remove
                    </Button>
                  </Col>
                </Row>
              ))}
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

export default Orders

