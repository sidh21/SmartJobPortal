// src/pages/Landing/Landing.tsx
import React from "react";
import { useNavigate } from "react-router-dom";
import styles from "./Landing.module.scss";

const Landing: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className={styles.container}>
      <div className={styles.hero}>
        <h1>
          Find Your Dream Job with <span>AI Power</span> 🚀
        </h1>
        <p>
          SmartJobPortal uses Google Gemini AI to match candidates with the
          perfect job — faster, smarter, better.
        </p>
        <div className={styles.actions}>
          <button className={styles.primary} onClick={() => navigate("/jobs")}>
            Browse Jobs
          </button>
          <button
            className={styles.secondary}
            onClick={() => navigate("/register")}
          >
            Get Started Free
          </button>
        </div>
      </div>

      <div className={styles.features}>
        <div className={styles.feature}>
          <div className={styles.icon}>🤖</div>
          <h3>AI Resume Screening</h3>
          <p>Gemini AI analyzes resumes and ranks candidates automatically</p>
        </div>
        <div className={styles.feature}>
          <div className={styles.icon}>✍️</div>
          <h3>AI Job Description</h3>
          <p>Generate professional job descriptions in seconds with AI</p>
        </div>
        <div className={styles.feature}>
          <div className={styles.icon}>🎯</div>
          <h3>Smart Matching</h3>
          <p>Get matched with jobs that fit your skills perfectly</p>
        </div>
      </div>
    </div>
  );
};

export default Landing;
