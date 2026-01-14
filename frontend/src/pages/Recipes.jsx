import { useState, useEffect } from 'react'
import { Container, Table, Button, Modal, Form, Pagination } from 'react-bootstrap'
import { recipeService, menuService, inventoryService } from '../services/api'
import { toast } from 'react-toastify'

const Recipes = () => {
  const [recipes, setRecipes] = useState([])
  const [menus, setMenus] = useState([])
  const [inventory, setInventory] = useState([])
  const [showModal, setShowModal] = useState(false)
  const [editingRecipe, setEditingRecipe] = useState(null)
  const [loading, setLoading] = useState(false)
  const [currentPage, setCurrentPage] = useState(1)
  const [pageSize] = useState(10)
  const [totalPages, setTotalPages] = useState(1)
  const [formData, setFormData] = useState({
    menuId: '',
    itemId: '',
    quantityRequired: ''
  })

  useEffect(() => {
    loadRecipes()
    loadMenus()
    loadInventory()
  }, [currentPage])

  const loadRecipes = async () => {
    try {
      const response = await recipeService.getAll(currentPage, pageSize)
      setRecipes(response.data.items || [])
      setTotalPages(response.data.totalPages || 1)
    } catch (error) {
      toast.error('Failed to load recipes')
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

  const loadInventory = async () => {
    try {
      const response = await inventoryService.getAll(1, 100)
      setInventory(response.data.items || [])
    } catch (error) {
      toast.error('Failed to load inventory')
    }
  }

  const handleShow = (recipe = null) => {
    if (recipe) {
      setEditingRecipe(recipe)
      // Convert to string to match select value
      setFormData({
        menuId: String(recipe.menuId || recipe.id || ''),
        itemId: String(recipe.itemId || ''),
        quantityRequired: recipe.quantityRequired || ''
      })
    } else {
      setEditingRecipe(null)
      setFormData({
        menuId: '',
        itemId: '',
        quantityRequired: ''
      })
    }
    setShowModal(true)
  }

  const handleClose = () => {
    setShowModal(false)
    setEditingRecipe(null)
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)

    try {
      // Ensure menuId and itemId are properly converted to integers
      const menuIdInt = parseInt(formData.menuId, 10)
      const itemIdInt = parseInt(formData.itemId, 10)
      const quantityFloat = parseFloat(formData.quantityRequired)
      
      if (isNaN(menuIdInt) || isNaN(itemIdInt) || isNaN(quantityFloat)) {
        toast.error('Please fill in all required fields with valid values')
        setLoading(false)
        return
      }

      const submitData = {
        menuId: menuIdInt,
        itemId: itemIdInt,
        quantityRequired: quantityFloat
      }
      
      if (editingRecipe) {
        const recipeId = editingRecipe.recipeId || editingRecipe.id
        await recipeService.update(recipeId, submitData)
        toast.success('Recipe updated successfully')
      } else {
        await recipeService.create(submitData)
        toast.success('Recipe created successfully')
      }
      handleClose()
      loadRecipes()
    } catch (error) {
      const errorMessage = error.response?.data?.message || error.response?.data || error.message || 'Operation failed'
      toast.error(errorMessage)
      console.error('Error:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleDelete = async (recipe) => {
    if (window.confirm('Are you sure you want to delete this recipe?')) {
      try {
        const recipeId = recipe.recipeId || recipe.id
        await recipeService.delete(recipeId)
        toast.success('Recipe deleted successfully')
        loadRecipes()
      } catch (error) {
        const errorMessage = error.response?.data?.message || error.response?.data || error.message || 'Failed to delete recipe'
        toast.error(errorMessage)
        console.error('Delete error:', error)
      }
    }
  }

  const getMenuName = (menuId) => {
    const menu = menus.find(m => (m.menuId || m.id) === menuId)
    return menu ? menu.menuName : `Menu ID: ${menuId}`
  }

  const getItemName = (itemId) => {
    const item = inventory.find(i => i.id === itemId)
    return item ? item.itemName : `Item ID: ${itemId}`
  }

  return (
    <Container>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>Recipe Management</h1>
        <Button variant="primary" onClick={() => handleShow()}>
          Add Recipe
        </Button>
      </div>

      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Menu</th>
            <th>Inventory Item</th>
            <th>Quantity Required</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {recipes.map((recipe) => (
            <tr key={recipe.recipeId || recipe.id}>
              <td>{getMenuName(recipe.menuId)}</td>
              <td>{getItemName(recipe.itemId)}</td>
              <td>{recipe.quantityRequired}</td>
              <td>
                <Button
                  variant="warning"
                  size="sm"
                  className="me-2"
                  onClick={() => handleShow(recipe)}
                >
                  Edit
                </Button>
                <Button
                  variant="danger"
                  size="sm"
                  onClick={() => handleDelete(recipe)}
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
          <Modal.Title>{editingRecipe ? 'Edit Recipe' : 'Add Recipe'}</Modal.Title>
        </Modal.Header>
        <Form onSubmit={handleSubmit}>
          <Modal.Body>
            <Form.Group className="mb-3">
              <Form.Label>Menu</Form.Label>
              <Form.Select
                value={formData.menuId}
                onChange={(e) => setFormData({ ...formData, menuId: e.target.value })}
                required
              >
                <option value="">Select Menu</option>
                {menus.map((menu) => {
                  const menuIdValue = menu.menuId || menu.id
                  return (
                    <option key={menuIdValue} value={String(menuIdValue)}>
                      {menu.menuName}
                    </option>
                  )
                })}
              </Form.Select>
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Inventory Item</Form.Label>
              <Form.Select
                value={formData.itemId}
                onChange={(e) => setFormData({ ...formData, itemId: e.target.value })}
                required
              >
                <option value="">Select Inventory Item</option>
                {inventory.map((item) => (
                  <option key={item.id} value={String(item.id)}>
                    {item.itemName} ({item.unit})
                  </option>
                ))}
              </Form.Select>
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Quantity Required</Form.Label>
              <Form.Control
                type="number"
                step="0.01"
                value={formData.quantityRequired}
                onChange={(e) => setFormData({ ...formData, quantityRequired: e.target.value })}
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

export default Recipes

