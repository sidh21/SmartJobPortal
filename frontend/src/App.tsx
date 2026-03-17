// src/App.tsx
import React from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Navbar from './components/Navbar/Navbar';
import ProtectedRoute from './components/ProtectedRoute/ProtectedRoute';
import { AuthProvider } from './context/AuthContext';
import AIDashboard from './pages/AIDashboard/AIDashboard';
import Apply from './pages/Apply/Apply';
import EmployerDashboard from './pages/EmployerDashboard/EmployerDashboard';
import JobDetail from './pages/JobDetail/JobDetail';
import Jobs from './pages/Jobs/Jobs';
import Landing from './pages/Landing/Landing';
import Login from './pages/Login/Login';
import Register from './pages/Register/Register';
import './styles/global.scss';

const App: React.FC = () => {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Navbar />
        <Routes>
          {/* Public Routes */}
          <Route path="/" element={<Landing />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />

          {/* Protected Routes — any logged in user */}
          <Route
            path="/jobs"
            element={
              <ProtectedRoute>
                <Jobs />
              </ProtectedRoute>
            }
          />
          <Route
            path="/jobs/:id"
            element={
              <ProtectedRoute>
                <JobDetail />
              </ProtectedRoute>
            }
          />

          {/* Candidate only */}
          <Route
            path="/apply/:id"
            element={
              <ProtectedRoute allowedRoles={['Candidate']}>
                <Apply />
              </ProtectedRoute>
            }
          />

          {/* Employer only */}
          <Route
            path="/employer-dashboard"
            element={
              <ProtectedRoute allowedRoles={['Employer']}>
                <EmployerDashboard />
              </ProtectedRoute>
            }
          />
          <Route
            path="/ai-dashboard"
            element={
              <ProtectedRoute allowedRoles={['Employer']}>
                <AIDashboard />
              </ProtectedRoute>
            }
          />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
};

export default App;