# Food Management System - React Frontend

This is the React frontend for the Food Management System built with Vite, React, React Router, and Bootstrap.

## Features

- **Authentication**: Login with JWT token support
- **User Management**: Admin can manage users (Admin only)
- **Menu Management**: View and manage menu items (Admin can create/edit/delete, Cashier can view)
- **Inventory Management**: Manage inventory items (Admin only)
- **Order Management**: Create and manage orders (Cashier and Admin)
- **Recipe Management**: Manage recipes linking menus to inventory items (Admin only)
- **Reports**: Generate daily reports and low stock reports (Admin only)

## Installation

1. Navigate to the frontend directory:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

## Configuration

The API base URL is configured in `src/services/api.js`. By default, it points to:
- `http://localhost:5099/api`

Make sure your .NET backend is running on this port, or update the `API_BASE_URL` constant in the api.js file.

## Running the Application

Start the development server:
```bash
npm run dev
```

The application will be available at `http://localhost:5173`

## Building for Production

To build the application for production:
```bash
npm run build
```

The built files will be in the `dist` directory.

## Project Structure

```
frontend/
├── src/
│   ├── components/       # Reusable components
│   │   ├── Layout.jsx
│   │   └── PrivateRoute.jsx
│   ├── contexts/         # React contexts
│   │   └── AuthContext.jsx
│   ├── pages/            # Page components
│   │   ├── Login.jsx
│   │   ├── Dashboard.jsx
│   │   ├── Users.jsx
│   │   ├── Menus.jsx
│   │   ├── Inventory.jsx
│   │   ├── Orders.jsx
│   │   ├── Recipes.jsx
│   │   └── Reports.jsx
│   ├── services/         # API services
│   │   └── api.js
│   ├── App.jsx           # Main app component
│   ├── main.jsx          # Entry point
│   └── index.css         # Global styles
├── index.html
├── package.json
└── vite.config.js
```

## User Roles

- **Admin**: Full access to all features
- **Cashier**: Can view menus and manage orders

## API Integration

The frontend communicates with the .NET backend API. Make sure:
1. The backend is running
2. CORS is properly configured in the backend
3. The API base URL matches your backend configuration

## Technologies Used

- React 18
- React Router 6
- Bootstrap 5
- React Bootstrap
- Axios
- Vite
- React Toastify

