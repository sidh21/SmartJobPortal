import React from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import styles from "./Navbar.module.scss";

const Navbar: React.FC = () => {
  const { user, logout, isAuthenticated, isEmployer } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <nav className={styles.navbar}>
      <div className={styles.brand}>
        <Link to="/">🚀 SmartJobPortal</Link>
      </div>

      <div className={styles.links}>
        <Link to="/jobs">Jobs</Link>

        {isAuthenticated && isEmployer && (
          <>
            <Link to="/employer-dashboard">Dashboard</Link>
            <Link to="/ai-dashboard">AI Tools</Link>
          </>
        )}

        {isAuthenticated ? (
          <div className={styles.userInfo}>
            <span>👤 {user?.fullName}</span>
            <span className={styles.role}>{user?.role}</span>
            <button onClick={handleLogout}>Logout</button>
          </div>
        ) : (
          <>
            <Link to="/login">Login</Link>
            <Link to="/register">Register</Link>
          </>
        )}
      </div>
    </nav>
  );
};

export default Navbar;
