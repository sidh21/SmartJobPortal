// src/pages/Jobs/Jobs.tsx
import React, { useEffect, useState } from "react";
import { getAllJobsApi } from "../../api/jobApi";
import JobCard from "../../components/JobCard/JobCard";
import { Job } from "../../types";
import styles from "./Jobs.module.scss";

const Jobs: React.FC = () => {
  const [jobs, setJobs] = useState<Job[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [search, setSearch] = useState("");

  useEffect(() => {
    fetchJobs();
  }, []);

  const fetchJobs = async () => {
    try {
      const response = await getAllJobsApi();
      if (response.success) {
        setJobs(response.data);
      }
    } catch (err) {
      setError("Failed to load jobs. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  const filteredJobs = jobs.filter(
    (job) =>
      job.title.toLowerCase().includes(search.toLowerCase()) ||
      job.company.toLowerCase().includes(search.toLowerCase()) ||
      job.location.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1>Find Your Dream Job 🚀</h1>
        <p>{jobs.length} jobs available</p>
        <input
          type="text"
          placeholder="Search by title, company or location..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          className={styles.search}
        />
      </div>

      {loading && <div className={styles.loading}>Loading jobs...</div>}
      {error && <div className={styles.error}>{error}</div>}

      <div className={styles.grid}>
        {filteredJobs.map((job) => (
          <JobCard key={job.jobId} job={job} />
        ))}
      </div>

      {!loading && filteredJobs.length === 0 && (
        <div className={styles.empty}>No jobs found matching your search.</div>
      )}
    </div>
  );
};

export default Jobs;
