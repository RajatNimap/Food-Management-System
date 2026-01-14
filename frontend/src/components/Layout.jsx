import { Outlet, Link, useLocation, useNavigate } from 'react-router-dom'
import { Navbar, Nav, Container, NavDropdown } from 'react-bootstrap'
import { useAuth } from '../contexts/AuthContext'

const Layout = () => {
  const { user, logout, isAdmin } = useAuth()
  const location = useLocation()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  const isActive = (path) => location.pathname === path

  return (
    <div className="d-flex flex-column" style={{ minHeight: '100vh' }}>
      <Navbar bg="dark" variant="dark" expand="lg" className="mb-3">
        <Container fluid>
          <Navbar.Brand as={Link} to="/dashboard">
            🍽️ Food Management System
          </Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            <Nav className="me-auto">
              <Nav.Link as={Link} to="/dashboard" active={isActive('/dashboard')}>
                Dashboard
              </Nav.Link>
              <Nav.Link as={Link} to="/menus" active={isActive('/menus')}>
                Menus
              </Nav.Link>
              <Nav.Link as={Link} to="/orders" active={isActive('/orders')}>
                Orders
              </Nav.Link>
              {isAdmin() && (
                <>
                  <Nav.Link as={Link} to="/inventory" active={isActive('/inventory')}>
                    Inventory
                  </Nav.Link>
                  <Nav.Link as={Link} to="/recipes" active={isActive('/recipes')}>
                    Recipes
                  </Nav.Link>
                  <Nav.Link as={Link} to="/users" active={isActive('/users')}>
                    Users
                  </Nav.Link>
                  <Nav.Link as={Link} to="/reports" active={isActive('/reports')}>
                    Reports
                  </Nav.Link>
                </>
              )}
            </Nav>
            <Nav>
              <Nav.Link as={Link} to="/profile" active={isActive('/profile')}>
                Profile
              </Nav.Link>
              <NavDropdown title={user?.email || 'User'} id="user-nav-dropdown" align="end">
                <NavDropdown.Item as={Link} to="/profile">Profile</NavDropdown.Item>
                <NavDropdown.Divider />
                <NavDropdown.Item onClick={handleLogout}>Logout</NavDropdown.Item>
              </NavDropdown>
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
      <Container fluid className="flex-grow-1">
        <Outlet />
      </Container>
    </div>
  )
}

export default Layout

