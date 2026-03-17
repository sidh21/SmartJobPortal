import React from "react";
import { useNavigate } from "react-router-dom";
import { Job } from "../../types";
import styles from "./JobCard.module.scss";

interface JobCardProps {
  job: Job;
}

const JobCard: React.FC<JobCardProps> = ({ job }) => {
  const navigate = useNavigate();

  return (
    <div className={styles.card}>
      <div className={styles.header}>
        <h3>{job.title}</h3>
        <span className={styles.jobType}>{job.jobType}</span>
      </div>

      <div className={styles.company}>🏢 {job.company}</div>

      <div className={styles.details}>
        <span>📍 {job.location}</span>
        {job.salary && <span>💰 ₹{job.salary.toLocaleString()}</span>}
      </div>

      <p className={styles.description}>
        {job.description.substring(0, 120)}...
      </p>

      <div className={styles.footer}>
        <span className={styles.date}>
          🕒 {new Date(job.createdAt).toLocaleDateString()}
        </span>
        <button
          className={styles.applyBtn}
          onClick={() => navigate(`/jobs/${job.jobId}`)}
        >
          View Details
        </button>
      </div>
    </div>
  );
};

export default JobCard;
