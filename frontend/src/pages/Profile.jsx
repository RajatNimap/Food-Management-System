import { useState, useEffect } from 'react'
import { Container, Card, Form, Button, Alert, Row, Col } from 'react-bootstrap'
import { userService } from '../services/api'
import { toast } from 'react-toastify'
import { useAuth } from '../contexts/AuthContext'

const Profile = () => {
  const { user, isAdmin } = useAuth()
  const [loading, setLoading] = useState(false)
  const [userDetails, setUserDetails] = useState(null)
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: '',
    confirmPassword: ''
  })
  const [showPasswordForm, setShowPasswordForm] = useState(false)

  useEffect(() => {
    loadUserDetails()
  }, [user])

  const loadUserDetails = async () => {
    if (!user?.id) return
    
    try {
      // Try to fetch user details if admin, otherwise use token data
      if (isAdmin()) {
        try {
          const response = await userService.getById(user.id)
          setUserDetails(response.data)
          setFormData({
            name: response.data.name || user.name || '',
            email: response.data.email || user.email || '',
            password: '',
            confirmPassword: ''
          })
        } catch (error) {
          // If fetch fails, use token data
          setFormData({
            name: user.name || '',
            email: user.email || '',
            password: '',
            confirmPassword: ''
          })
        }
      } else {
        // For cashiers, use token data
        setFormData({
          name: user.name || '',
          email: user.email || '',
          password: '',
          confirmPassword: ''
        })
      }
    } catch (error) {
      console.error('Error loading user details:', error)
      // Fallback to token data
      setFormData({
        name: user.name || '',
        email: user.email || '',
        password: '',
        confirmPassword: ''
      })
    }
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)

    if (showPasswordForm) {
      // Password change
      if (formData.password !== formData.confirmPassword) {
        toast.error('Passwords do not match')
        setLoading(false)
        return
      }
      if (formData.password.length < 6) {
        toast.error('Password must be at least 6 characters long')
        setLoading(false)
        return
      }
    }

    try {
      if (!user?.id) {
        toast.error('User ID not available')
        setLoading(false)
        return
      }

      // Only admins can update profile via API
      if (isAdmin()) {
        const updateData = {
          name: formData.name,
          email: formData.email,
          password: formData.password || undefined,
          role: user.role
        }
        
        // Remove password if not changing
        if (!showPasswordForm || !formData.password) {
          delete updateData.password
        }

        await userService.update(user.id, updateData)
        toast.success(showPasswordForm ? 'Password updated successfully' : 'Profile updated successfully')
        
        if (showPasswordForm) {
          setShowPasswordForm(false)
          setFormData({ ...formData, password: '', confirmPassword: '' })
        }
      } else {
        toast.error('Profile updates require admin privileges. Please contact an administrator.')
      }
    } catch (error) {
      const errorMessage = error.response?.data?.message || error.response?.data || error.message || 'Operation failed'
      toast.error(errorMessage)
      console.error('Error:', error)
    } finally {
      setLoading(false)
    }
  }

  return (
    <Container>
      <Row className="justify-content-center">
        <Col md={8} lg={6}>
          <Card className="mt-4">
            <Card.Header>
              <h3 className="mb-0">My Profile</h3>
            </Card.Header>
            <Card.Body>
              <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3">
                  <Form.Label>Name</Form.Label>
                  <Form.Control
                    type="text"
                    value={formData.name}
                    onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                    disabled={!isAdmin()}
                    required
                  />
                </Form.Group>

                <Form.Group className="mb-3">
                  <Form.Label>Email</Form.Label>
                  <Form.Control
                    type="email"
                    value={formData.email}
                    onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                    disabled={!isAdmin()}
                    required
                  />
                </Form.Group>

                <Form.Group className="mb-3">
                  <Form.Label>Role</Form.Label>
                  <Form.Control
                    type="text"
                    value={user?.role || ''}
                    disabled
                    className="bg-light"
                  />
                </Form.Group>

                {!showPasswordForm ? (
                  <div className="mb-3">
                    <Button
                      variant="outline-primary"
                      onClick={() => setShowPasswordForm(true)}
                    >
                      Change Password
                    </Button>
                  </div>
                ) : (
                  <>
                    <hr />
                    <h5>Change Password</h5>
                    <Form.Group className="mb-3">
                      <Form.Label>New Password</Form.Label>
                      <Form.Control
                        type="password"
                        value={formData.password}
                        onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                        required
                        minLength={6}
                      />
                    </Form.Group>

                    <Form.Group className="mb-3">
                      <Form.Label>Confirm Password</Form.Label>
                      <Form.Control
                        type="password"
                        value={formData.confirmPassword}
                        onChange={(e) => setFormData({ ...formData, confirmPassword: e.target.value })}
                        required
                        minLength={6}
                      />
                    </Form.Group>

                    <div className="mb-3">
                      <Button
                        variant="outline-secondary"
                        onClick={() => {
                          setShowPasswordForm(false)
                          setFormData({ ...formData, password: '', confirmPassword: '' })
                        }}
                      >
                        Cancel
                      </Button>
                    </div>
                  </>
                )}

                {!isAdmin() && (
                  <Alert variant="info" className="mt-3">
                    <small>Note: Profile updates require admin privileges. Please contact an administrator to update your profile.</small>
                  </Alert>
                )}

                <div className="d-grid gap-2 mt-4">
                  <Button
                    variant="primary"
                    type="submit"
                    disabled={loading || (!isAdmin() && !showPasswordForm)}
                  >
                    {loading ? 'Saving...' : showPasswordForm ? 'Update Password' : isAdmin() ? 'Update Profile' : 'View Profile'}
                  </Button>
                </div>
              </Form>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  )
}

export default Profile

