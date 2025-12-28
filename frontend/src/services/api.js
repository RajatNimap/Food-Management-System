import axios from 'axios'

const API_BASE_URL = 'https://localhost:7125/api'

// Create axios instance
const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

// Request interceptor to add token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('accessToken')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

// Response interceptor for token refresh
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true

      try {
        const refreshToken = localStorage.getItem('refreshToken')
        if (refreshToken) {
          const response = await axios.post(
            `${API_BASE_URL}/Auth/RefreshToken?token=${refreshToken}`
          )
          const { accessToken, refreshToken: newRefreshToken } = response.data
          localStorage.setItem('accessToken', accessToken)
          localStorage.setItem('refreshToken', newRefreshToken)
          originalRequest.headers.Authorization = `Bearer ${accessToken}`
          return api(originalRequest)
        }
      } catch (refreshError) {
        localStorage.removeItem('accessToken')
        localStorage.removeItem('refreshToken')
        window.location.href = '/login'
        return Promise.reject(refreshError)
      }
    }

    return Promise.reject(error)
  }
)

// Auth Service
export const authService = {
  login: (email, password) => api.post('/Auth', { email, password }),
  refreshToken: (token) => api.post(`/Auth/RefreshToken?token=${token}`),
}

// User Service
export const userService = {
  getAll: (pageNumber, pageSize) => api.get(`/User?pageNumber=${pageNumber}&pageSize=${pageSize}`),
  getById: (id) => api.get(`/User/${id}`),
  create: (data) => api.post('/User', data),
  update: (id, data) => api.put(`/User/${id}`, data),
  delete: (id) => api.delete(`/User/${id}`),
}

// Menu Service
export const menuService = {
  getAll: (pnum, psize) => api.get(`/Menu?pnum=${pnum}&psize=${psize}`),
  getById: (id) => api.get(`/Menu/${id}`),
  create: (data) => api.post('/Menu', data),
  update: (id, data) => api.put(`/Menu/${id}`, data),
  delete: (id) => api.delete(`/Menu/${id}`),
}

// Inventory Service
export const inventoryService = {
  getAll: (pnum, psize) => api.get(`/Inventory?pnum=${pnum}&psize=${psize}`),
  getById: (id) => api.get(`/Inventory/${id}`),
  create: (data) => api.post('/Inventory', data),
  update: (id, data) => api.put(`/Inventory/${id}`, data),
  delete: (id) => api.delete(`/Inventory/${id}`),
}

// Order Service
export const orderService = {
  getAll: (pnum, psize) => api.get(`/Order?pnum=${pnum}&psize=${psize}`),
  getById: (id) => api.get(`/Order/${id}`),
  create: (data) => api.post('/Order/PlacingOrder', data),
  update: (id, data) => api.put(`/Order/${id}`, data),
  delete: (id) => api.delete(`/Order/${id}`),
}

// Recipe Service
export const recipeService = {
  getAll: (pnum, psize) => api.get(`/Recipe?pnum=${pnum}&psize=${psize}`),
  getById: (id) => api.get(`/Recipe/${id}`),
  create: (data) => api.post('/Recipe', data),
  update: (id, data) => api.put(`/Recipe/${id}`, data),
  delete: (id) => api.delete(`/Recipe/${id}`),
}

// Report Service
export const reportService = {
  getDailyReport: (date) => api.get(`/Report?date=${date}`),
  getLowStockReport: () => api.get('/Report/LowStockReport'),
  downloadDailyReport: (reportDate) => 
    api.get(`/Report/DailyReport?reportDate=${reportDate}`, { responseType: 'blob' }),
  downloadLowStockReport: () => 
    api.get('/Report/LowStockExcel', { responseType: 'blob' }),
}

export default api

