# Setup Instructions

## Prerequisites

- Node.js (v16 or higher)
- npm or yarn
- .NET backend running on `http://localhost:5099`

## Installation Steps

1. **Navigate to the frontend directory:**
   ```bash
   cd frontend
   ```

2. **Install dependencies:**
   ```bash
   npm install
   ```

3. **Start the development server:**
   ```bash
   npm run dev
   ```

4. **Access the application:**
   Open your browser and navigate to `http://localhost:5173`

## Backend Configuration

Make sure your .NET backend is configured correctly:

1. The backend should be running on `http://localhost:5099` (or update the API_BASE_URL in `src/services/api.js`)
2. CORS should be enabled in the backend (already configured in Program.cs)
3. The backend should have a valid JWT configuration

## Default Login

You'll need to create a user account through the backend first, or use an existing account to login.

## Troubleshooting

### CORS Issues
If you encounter CORS errors, make sure:
- The backend CORS policy allows requests from `http://localhost:5173`
- The backend is running and accessible

### API Connection Issues
- Verify the backend is running on the correct port
- Check the API_BASE_URL in `src/services/api.js`
- Check browser console for detailed error messages

### Authentication Issues
- Make sure you're using valid credentials
- Check that the JWT token is being stored in localStorage
- Verify the token format matches what the backend expects

## Building for Production

```bash
npm run build
```

The production build will be in the `dist` directory.

