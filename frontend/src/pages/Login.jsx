import { useState } from 'react'
import { Container, Card, Form, Button } from 'react-bootstrap'
import { useAuth } from '../contexts/AuthContext'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import './Login.css'

const Login = () => {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [loading, setLoading] = useState(false)
  const { login } = useAuth()
  const navigate = useNavigate()

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)

    const result = await login(email, password)
    
    if (result.success) {
      toast.success('Login successful!')
      navigate('/dashboard')
    } else {
      toast.error(result.message || 'Login failed')
    }
    
    setLoading(false)
  }

  return (
    <div className="login-wrapper">
      <div className="login-background">
        <div className="shape shape-1"></div>
        <div className="shape shape-2"></div>
        <div className="shape shape-3"></div>
      </div>
      <Container className="d-flex align-items-center justify-content-center login-container">
        <Card className="login-card">
          <Card.Body className="p-4">
            <div className="text-center mb-4">
              <div className="login-icon">🍽️</div>
              <Card.Title className="login-title mt-3">Food Management System</Card.Title>
              <p className="text-muted mb-0">Please login to continue</p>
            </div>
            <Form onSubmit={handleSubmit}>
              <Form.Group className="mb-3">
                <Form.Label className="fw-semibold">Email</Form.Label>
                <Form.Control
                  type="email"
                  placeholder="Enter your email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                  className="form-control-custom"
                />
              </Form.Group>
              <Form.Group className="mb-4">
                <Form.Label className="fw-semibold">Password</Form.Label>
                <Form.Control
                  type="password"
                  placeholder="Enter your password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                  className="form-control-custom"
                />
              </Form.Group>
              <Button 
                variant="primary" 
                type="submit" 
                className="w-100 login-button" 
                disabled={loading}
              >
                {loading ? 'Logging in...' : 'Login'}
              </Button>
            </Form>
          </Card.Body>
        </Card>
      </Container>
    </div>
  )
}

export default Login

