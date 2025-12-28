import { createContext, useContext, useState, useEffect } from 'react'
import { authService } from '../services/api'

const AuthContext = createContext()

export const useAuth = () => {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider')
  }
  return context
}

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null)
  const [token, setToken] = useState(localStorage.getItem('accessToken'))
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const storedToken = localStorage.getItem('accessToken')
    if (storedToken) {
      setToken(storedToken)
      // Decode token to get user info (simple base64 decode)
      try {
        const payload = JSON.parse(atob(storedToken.split('.')[1]))
        setUser({
          id: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || payload.nameid,
          name: payload.name || payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
          email: payload.email || payload.sub,
          role: payload.role || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
        })
      } catch (e) {
        console.error('Error decoding token:', e)
      }
    }
    setLoading(false)
  }, [])

  const login = async (email, password) => {
    try {
      const response = await authService.login(email, password)
      const { accessToken, refreshToken } = response.data
      
      localStorage.setItem('accessToken', accessToken)
      localStorage.setItem('refreshToken', refreshToken)
      setToken(accessToken)
      
      // Decode token to get user info
      const payload = JSON.parse(atob(accessToken.split('.')[1]))
      const userData = {
        id: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || payload.nameid,
        name: payload.name || payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
        email: payload.email || payload.sub,
        role: payload.role || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
      }
      setUser(userData)
      
      return { success: true }
    } catch (error) {
      return { 
        success: false, 
        message: error.response?.data?.message || error.message || 'Login failed' 
      }
    }
  }

  const logout = () => {
    localStorage.removeItem('accessToken')
    localStorage.removeItem('refreshToken')
    setToken(null)
    setUser(null)
  }

  const refreshToken = async () => {
    try {
      const refreshTokenValue = localStorage.getItem('refreshToken')
      if (!refreshTokenValue) {
        throw new Error('No refresh token available')
      }
      
      const response = await authService.refreshToken(refreshTokenValue)
      const { accessToken, refreshToken: newRefreshToken } = response.data
      
      localStorage.setItem('accessToken', accessToken)
      localStorage.setItem('refreshToken', newRefreshToken)
      setToken(accessToken)
      
      return true
    } catch (error) {
      logout()
      return false
    }
  }

  const isAdmin = () => {
    return user?.role === 'Admin'
  }

  const isCashier = () => {
    return user?.role === 'Cashier'
  }

  const value = {
    user,
    token,
    login,
    logout,
    refreshToken,
    isAdmin,
    isCashier,
    loading
  }

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

