// src/pages/JobDetail/JobDetail.tsx
import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getJobByIdApi } from "../../api/jobApi";
import { useAuth } from "../../context/AuthContext";
import { Job } from "../../types";
import styles from "./JobDetail.module.scss";

const JobDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { isAuthenticated, isCandidate } = useAuth();
  const [job, setJob] = useState<Job | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchJob();
  }, [id]);

  const fetchJob = async () => {
    try {
      const response = await getJobByIdApi(Number(id));
      if (response.success) setJob(response.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div className={styles.loading}>Loading...</div>;
  if (!job) return <div className={styles.loading}>Job not found.</div>;

  return (
    <div className={styles.container}>
      <div className={styles.card}>
        <div className={styles.header}>
          <div>
            <h1>{job.title}</h1>
            <p className={styles.company}>🏢 {job.company}</p>
          </div>
          <span className={styles.jobType}>{job.jobType}</span>
        </div>

        <div className={styles.meta}>
          <span>📍 {job.location}</span>
          {job.salary && <span>💰 ₹{job.salary.toLocaleString()} / year</span>}
          <span>🕒 Posted {new Date(job.createdAt).toLocaleDateString()}</span>
        </div>

        <div className={styles.section}>
          <h3>Job Description</h3>
          <p>{job.description}</p>
        </div>

        <div className={styles.actions}>
          <button className={styles.backBtn} onClick={() => navigate("/jobs")}>
            ← Back to Jobs
          </button>

          {isAuthenticated && isCandidate && (
            <button
              className={styles.applyBtn}
              onClick={() => navigate(`/apply/${job.jobId}`)}
            >
              Apply Now 🚀
            </button>
          )}

          {!isAuthenticated && (
            <button
              className={styles.applyBtn}
              onClick={() => navigate("/login")}
            >
              Login to Apply
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

export default JobDetail;
