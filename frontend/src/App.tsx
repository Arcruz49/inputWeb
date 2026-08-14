import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom'
import { AuthProvider } from './auth/AuthContext'
import { AppLayout } from './components/AppLayout'
import { GuestRoute, ProtectedRoute } from './components/ProtectedRoute'
import { Login } from './pages/Login'
import { Register } from './pages/Register'
import { Records } from './pages/Records'
import { RecordDetail } from './pages/RecordDetail'

export function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route element={<GuestRoute />}>
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
          </Route>

          <Route element={<ProtectedRoute />}>
            <Route element={<AppLayout />}>
              <Route path="/records" element={<Records />} />
              <Route path="/records/:id" element={<RecordDetail />} />
            </Route>
          </Route>

          <Route path="*" element={<Navigate to="/records" replace />} />
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  )
}
