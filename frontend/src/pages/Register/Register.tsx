import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { registerApi } from "../../api/authApi";
import { useAuth } from "../../context/AuthContext";
import styles from "./Register.module.scss";

const Register: React.FC = () => {
  const navigate = useNavigate();
  const { login } = useAuth();

  const [form, setForm] = useState({
    fullName: "",
    email: "",
    password: "",
    role: "Candidate",
  });
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError("");

    try {
      const response = await registerApi(
        form.fullName,
        form.email,
        form.password,
        form.role
      );

      if (response.success) {
        login(response.data);
        navigate("/jobs");
      } else {
        setError(response.errors[0] || "Registration failed");
      }
    } catch (err: any) {
      setError(err.response?.data?.errors?.[0] || "Something went wrong!");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      <div className={styles.card}>
        <h2>Create Account 🚀</h2>
        <p>Join SmartJobPortal today</p>

        {error && <div className={styles.error}>{error}</div>}

        <form onSubmit={handleSubmit}>
          <div className={styles.field}>
            <label>Full Name</label>
            <input
              type="text"
              name="fullName"
              placeholder="Enter your full name"
              value={form.fullName}
              onChange={handleChange}
              required
            />
          </div>

          <div className={styles.field}>
            <label>Email</label>
            <input
              type="email"
              name="email"
              placeholder="Enter your email"
              value={form.email}
              onChange={handleChange}
              required
            />
          </div>

          <div className={styles.field}>
            <label>Password</label>
            <input
              type="password"
              name="password"
              placeholder="Min 6 characters"
              value={form.password}
              onChange={handleChange}
              required
            />
          </div>

          <div className={styles.field}>
            <label>I am a</label>
            <select name="role" value={form.role} onChange={handleChange}>
              <option value="Candidate">Candidate</option>
              <option value="Employer">Employer</option>
            </select>
          </div>

          <button type="submit" className={styles.btn} disabled={loading}>
            {loading ? "Creating account..." : "Register"}
          </button>
        </form>

        <p className={styles.link}>
          Already have an account? <Link to="/login">Login here</Link>
        </p>
      </div>
    </div>
  );
};

export default Register;
