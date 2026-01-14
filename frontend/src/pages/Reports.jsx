import { useState } from 'react'
import { Container, Card, Button, Form, Table, Row, Col, Alert } from 'react-bootstrap'
import { reportService } from '../services/api'
import { toast } from 'react-toastify'

const Reports = () => {
  const [dailyReport, setDailyReport] = useState(null)
  const [lowStockReport, setLowStockReport] = useState(null)
  const [reportDate, setReportDate] = useState(new Date().toISOString().split('T')[0])
  const [loading, setLoading] = useState(false)

  const handleDailyReport = async () => {
    setLoading(true)
    try {
      const response = await reportService.getDailyReport(reportDate)
      setDailyReport(response.data)
      toast.success('Daily report loaded successfully')
    } catch (error) {
      toast.error('Failed to load daily report')
    } finally {
      setLoading(false)
    }
  }
  
  const handleLowStockReport = async () => {
    setLoading(true)
    try {
      const response = await reportService.getLowStockReport()
      console.log('Low Stock Report Response:', response.data)
      setLowStockReport(response.data)
      const itemCount = response.data?.lowStockItems?.length || response.data?.LowStockItems?.length || 0
      if (itemCount > 0) {
        toast.success(`Low stock report loaded successfully - ${itemCount} items found`)
      } else {
        toast.info('No low stock items found')
      }
    } catch (error) {
      toast.error('Failed to load low stock report')
      console.error('Error loading low stock report:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleDownloadDailyReport = async () => {
    try {
      const response = await reportService.downloadDailyReport(reportDate)
      const url = window.URL.createObjectURL(new Blob([response.data]))
      const link = document.createElement('a')
      link.href = url
      link.setAttribute('download', `DailyReport_${reportDate}.xlsx`)
      document.body.appendChild(link)
      link.click()
      link.remove()
      toast.success('Report downloaded successfully')
    } catch (error) {
      toast.error('Failed to download report')
    }
  }

  const handleDownloadLowStockReport = async () => {
    try {
      const response = await reportService.downloadLowStockReport()
      const url = window.URL.createObjectURL(new Blob([response.data]))
      const link = document.createElement('a')
      link.href = url
      link.setAttribute('download', `LowStockReport_${new Date().toISOString().split('T')[0]}.xlsx`)
      document.body.appendChild(link)
      link.click()
      link.remove()
      toast.success('Report downloaded successfully')
    } catch (error) {
      toast.error('Failed to download report')
    }
  }

  return (
    <Container>
      <h1 className="mb-4">Reports</h1>

      <Row>
        <Col md={6}>
          <Card className="mb-4">
            <Card.Header>
              <h5>Daily Order Report</h5>
            </Card.Header>
            <Card.Body>
              <Form.Group className="mb-3">
                <Form.Label>Select Date</Form.Label>
                <Form.Control
                  type="date"
                  value={reportDate}
                  onChange={(e) => setReportDate(e.target.value)}
                />
              </Form.Group>
              <div className="d-flex gap-2">
                <Button
                  variant="primary"
                  onClick={handleDailyReport}
                  disabled={loading}
                >
                  {loading ? 'Loading...' : 'Generate Report'}
                </Button>
                <Button
                  variant="success"
                  onClick={handleDownloadDailyReport}
                  disabled={!dailyReport}
                >
                  Download Excel
                </Button>
              </div>
              {dailyReport && (
                <div className="mt-3">
                  <h6>Report Summary</h6>
                  <Table striped bordered size="sm">
                    <tbody>
                      <tr>
                        <td><strong>Total Orders:</strong></td>
                        <td>{dailyReport.totalOrders || 0}</td>
                      </tr>
                      <tr>
                        <td><strong>Total Revenue:</strong></td>
                        <td>${dailyReport.totalRevenue?.toFixed(2) || '0.00'}</td>
                      </tr>
                    </tbody>
                  </Table>
                  {dailyReport.orderItems && dailyReport.orderItems.length > 0 && (
                    <div className="mt-3">
                      <h6>Order Items</h6>
                      <Table striped bordered size="sm">
                        <thead>
                          <tr>
                            <th>Menu</th>
                            <th>Quantity</th>
                            <th>Total</th>
                          </tr>
                        </thead>
                        <tbody>
                          {dailyReport.orderItems.map((item, idx) => (
                            <tr key={idx}>
                              <td>Menu ID: {item.menuId}</td>
                              <td>{item.quantity}</td>
                              <td>${item.total?.toFixed(2) || '0.00'}</td>
                            </tr>
                          ))}
                        </tbody>
                      </Table>
                    </div>
                  )}
                </div>
              )}
            </Card.Body>
          </Card>
        </Col>

        <Col md={6}>
          <Card className="mb-4">
            <Card.Header>
              <h5>Low Stock Report</h5>
            </Card.Header>
            <Card.Body>
              <div className="d-flex gap-2 mb-3">
                <Button
                  variant="primary"
                  onClick={handleLowStockReport}
                  disabled={loading}
                >
                  {loading ? 'Loading...' : 'Generate Report'}
                </Button>
                <Button
                  variant="success"
                  onClick={handleDownloadLowStockReport}
                  disabled={!lowStockReport}
                >
                  Download Excel
                </Button>
              </div>
              {lowStockReport && (() => {
                // Handle both property name variations (case sensitivity from C# JSON)
                const items = lowStockReport.lowStockItems || lowStockReport.LowStockItems || []
                const totalItems = lowStockReport.totalLowStockItems || lowStockReport.TotalLowStockItems || items.length
                const criticalItems = lowStockReport.citicalItems || lowStockReport.CiticalItems || 0
                const summary = lowStockReport.lowStockSummaryModel || lowStockReport.LowStockSummaryModel
                
                if (items && items.length > 0) {
                  return (
                    <div className="mt-3">
                      <Alert variant="warning">
                        <strong>Total Low Stock Items:</strong> {totalItems}
                        {criticalItems > 0 && (
                          <span className="ms-2 text-danger">
                            (Critical: {criticalItems})
                          </span>
                        )}
                      </Alert>
                      {summary && (
                        <div className="mb-3">
                          <small className="text-muted">
                            Critical: {summary.criticalCount || summary.CriticalCount || 0} | 
                            Warning: {summary.warningCount || summary.WarningCount || 0}
                          </small>
                        </div>
                      )}
                      <Table striped bordered size="sm">
                        <thead>
                          <tr>
                            <th>Item Name</th>
                            <th>Available</th>
                            <th>Reorder Level</th>
                            <th>Deficit</th>
                            <th>Status</th>
                          </tr>
                        </thead>
                        <tbody>
                          {items.map((item, idx) => {
                            const itemName = item.itemName || item.ItemName
                            const quantityAvailable = item.quantityAvailable || item.QuantityAvailable || 0
                            const reorderLevel = item.reorderLevel || item.ReorderLevel || 0
                            const stockDeficit = item.stockDeficit || item.StockDeficit || (reorderLevel - quantityAvailable)
                            const status = item.status || item.Status
                            
                            return (
                              <tr key={item.id || item.Id || idx}>
                                <td>{itemName}</td>
                                <td>{quantityAvailable}</td>
                                <td>{reorderLevel}</td>
                                <td className="text-danger">
                                  {stockDeficit > 0 ? `-${Number(stockDeficit).toFixed(2)}` : '0'}
                                </td>
                                <td>
                                  <span className={`badge ${
                                    status === 'Out of Stock' ? 'bg-danger' : 
                                    status === 'Low Stock' ? 'bg-warning' : 
                                    'bg-success'
                                  }`}>
                                    {status || 'Unknown'}
                                  </span>
                                </td>
                              </tr>
                            )
                          })}
                        </tbody>
                      </Table>
                    </div>
                  )
                } else {
                  return (
                    <div className="mt-3">
                      <Alert variant="success">No low stock items found</Alert>
                    </div>
                  )
                }
              })()}
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  )
}

export default Reports

