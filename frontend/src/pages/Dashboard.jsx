import { Container, Row, Col, Card } from 'react-bootstrap'
import { Link } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'
import './Dashboard.css'

const Dashboard = () => {
  const { user } = useAuth()

  return (
    <div className="dashboard-wrapper">
      <Container>
        <div className="dashboard-header mb-5">
          <h1 className="dashboard-title">Dashboard</h1>
          <div className="welcome-card">
            <h3>Welcome back, {user?.email?.split('@')[0]}! 👋</h3>
            <p className="mb-0">You are logged in as <span className="role-badge">{user?.role}</span></p>
          </div>
        </div>

        <Row className="g-4">
          <Col md={4}>
            <Link to="/menus" className="dashboard-card-link">
              <Card className="dashboard-card h-100">
                <Card.Body className="text-center">
                  <div className="dashboard-icon">🍽️</div>
                  <Card.Title className="mt-3">Menus</Card.Title>
                  <Card.Text>Manage your menu items</Card.Text>
                </Card.Body>
              </Card>
            </Link>
          </Col>
          
          <Col md={4}>
            <Link to="/orders" className="dashboard-card-link">
              <Card className="dashboard-card h-100">
                <Card.Body className="text-center">
                  <div className="dashboard-icon">📋</div>
                  <Card.Title className="mt-3">Orders</Card.Title>
                  <Card.Text>View and manage orders</Card.Text>
                </Card.Body>
              </Card>
            </Link>
          </Col>

          {user?.role === 'Admin' && (
            <>
              <Col md={4}>
                <Link to="/inventory" className="dashboard-card-link">
                  <Card className="dashboard-card h-100">
                    <Card.Body className="text-center">
                      <div className="dashboard-icon">📦</div>
                      <Card.Title className="mt-3">Inventory</Card.Title>
                      <Card.Text>Manage inventory items</Card.Text>
                    </Card.Body>
                  </Card>
                </Link>
              </Col>
              
              <Col md={4}>
                <Link to="/recipes" className="dashboard-card-link">
                  <Card className="dashboard-card h-100">
                    <Card.Body className="text-center">
                      <div className="dashboard-icon">👨‍🍳</div>
                      <Card.Title className="mt-3">Recipes</Card.Title>
                      <Card.Text>Manage recipes</Card.Text>
                    </Card.Body>
                  </Card>
                </Link>
              </Col>
              
              <Col md={4}>
                <Link to="/users" className="dashboard-card-link">
                  <Card className="dashboard-card h-100">
                    <Card.Body className="text-center">
                      <div className="dashboard-icon">👥</div>
                      <Card.Title className="mt-3">Users</Card.Title>
                      <Card.Text>Manage system users</Card.Text>
                    </Card.Body>
                  </Card>
                </Link>
              </Col>
              
              <Col md={4}>
                <Link to="/reports" className="dashboard-card-link">
                  <Card className="dashboard-card h-100">
                    <Card.Body className="text-center">
                      <div className="dashboard-icon">📊</div>
                      <Card.Title className="mt-3">Reports</Card.Title>
                      <Card.Text>View system reports</Card.Text>
                    </Card.Body>
                  </Card>
                </Link>
              </Col>
            </>
          )}
        </Row>
      </Container>
    </div>
  )
}

export default Dashboard

