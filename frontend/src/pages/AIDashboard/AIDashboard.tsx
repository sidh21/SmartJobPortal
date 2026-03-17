// src/pages/AIDashboard/AIDashboard.tsx
import React, { useState } from "react";
import { generateJDApi, screenResumeApi } from "../../api/aiApi";
import styles from "./AIDashboard.module.scss";

const AIDashboard: React.FC = () => {
  const [activeTab, setActiveTab] = useState<"screen" | "generate">("generate");

  // Generate JD state
  const [jdForm, setJdForm] = useState({
    jobTitle: "",
    company: "",
    location: "",
    jobType: "FullTime",
  });
  const [generatedJD, setGeneratedJD] = useState("");
  const [jdLoading, setJdLoading] = useState(false);

  // Screen Resume state
  const [screenForm, setScreenForm] = useState({
    applicationId: 1,
    jobId: 1,
    resumeText: "",
    jobDescription: "",
  });
  const [screenResult, setScreenResult] = useState<any>(null);
  const [screenLoading, setScreenLoading] = useState(false);
  const [error, setError] = useState("");

  const handleGenerateJD = async (e: React.FormEvent) => {
    e.preventDefault();
    setJdLoading(true);
    setError("");
    try {
      const response = await generateJDApi(jdForm);
      if (response.success) setGeneratedJD(response.data);
    } catch (err) {
      setError("Failed to generate JD. Please try again.");
    } finally {
      setJdLoading(false);
    }
  };

  const handleScreenResume = async (e: React.FormEvent) => {
    e.preventDefault();
    setScreenLoading(true);
    setError("");
    try {
      const response = await screenResumeApi(screenForm);
      if (response.success) setScreenResult(response.data);
    } catch (err) {
      setError("Failed to screen resume. Please try again.");
    } finally {
      setScreenLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1>🤖 AI Dashboard</h1>
        <p>Powered by Google Gemini — Free AI Tools for Recruiters</p>
      </div>

      <div className={styles.tabs}>
        <button
          className={activeTab === "generate" ? styles.active : ""}
          onClick={() => setActiveTab("generate")}
        >
          ✍️ Generate Job Description
        </button>
        <button
          className={activeTab === "screen" ? styles.active : ""}
          onClick={() => setActiveTab("screen")}
        >
          🔍 Screen Resume
        </button>
      </div>

      {error && <div className={styles.error}>{error}</div>}

      {activeTab === "generate" && (
        <div className={styles.panel}>
          <form onSubmit={handleGenerateJD} className={styles.form}>
            <div className={styles.field}>
              <label>Job Title</label>
              <input
                type="text"
                placeholder="e.g. Senior .NET Developer"
                value={jdForm.jobTitle}
                onChange={(e) =>
                  setJdForm({ ...jdForm, jobTitle: e.target.value })
                }
                required
              />
            </div>

            <div className={styles.field}>
              <label>Company</label>
              <input
                type="text"
                placeholder="e.g. TechCorp Pune"
                value={jdForm.company}
                onChange={(e) =>
                  setJdForm({ ...jdForm, company: e.target.value })
                }
                required
              />
            </div>

            <div className={styles.field}>
              <label>Location</label>
              <input
                type="text"
                placeholder="e.g. Pune, MH"
                value={jdForm.location}
                onChange={(e) =>
                  setJdForm({ ...jdForm, location: e.target.value })
                }
                required
              />
            </div>

            <div className={styles.field}>
              <label>Job Type</label>
              <select
                value={jdForm.jobType}
                onChange={(e) =>
                  setJdForm({ ...jdForm, jobType: e.target.value })
                }
              >
                <option value="FullTime">Full Time</option>
                <option value="PartTime">Part Time</option>
                <option value="Remote">Remote</option>
              </select>
            </div>

            <button type="submit" className={styles.btn} disabled={jdLoading}>
              {jdLoading ? "⏳ Generating..." : "✨ Generate JD with AI"}
            </button>
          </form>

          {generatedJD && (
            <div className={styles.result}>
              <div className={styles.resultHeader}>
                <h3>Generated Job Description</h3>
                <button
                  onClick={() => navigator.clipboard.writeText(generatedJD)}
                >
                  📋 Copy
                </button>
              </div>
              <pre className={styles.jdText}>{generatedJD}</pre>
            </div>
          )}
        </div>
      )}

      {activeTab === "screen" && (
        <div className={styles.panel}>
          <form onSubmit={handleScreenResume} className={styles.form}>
            <div className={styles.row}>
              <div className={styles.field}>
                <label>Application ID</label>
                <input
                  type="number"
                  value={screenForm.applicationId}
                  onChange={(e) =>
                    setScreenForm({
                      ...screenForm,
                      applicationId: Number(e.target.value),
                    })
                  }
                  required
                />
              </div>

              <div className={styles.field}>
                <label>Job ID</label>
                <input
                  type="number"
                  value={screenForm.jobId}
                  onChange={(e) =>
                    setScreenForm({
                      ...screenForm,
                      jobId: Number(e.target.value),
                    })
                  }
                  required
                />
              </div>
            </div>

            <div className={styles.field}>
              <label>Job Description</label>
              <textarea
                placeholder="Paste the job description here..."
                value={screenForm.jobDescription}
                onChange={(e) =>
                  setScreenForm({
                    ...screenForm,
                    jobDescription: e.target.value,
                  })
                }
                rows={4}
                required
              />
            </div>

            <div className={styles.field}>
              <label>Resume Text</label>
              <textarea
                placeholder="Paste the candidate's resume here..."
                value={screenForm.resumeText}
                onChange={(e) =>
                  setScreenForm({
                    ...screenForm,
                    resumeText: e.target.value,
                  })
                }
                rows={6}
                required
              />
            </div>

            <button
              type="submit"
              className={styles.btn}
              disabled={screenLoading}
            >
              {screenLoading ? "⏳ Analyzing..." : "🔍 Screen Resume with AI"}
            </button>
          </form>

          {screenResult && (
            <div className={styles.screenResult}>
              <h3>AI Screening Result</h3>

              <div className={styles.scoreCard}>
                <div
                  className={styles.score}
                  style={{
                    color:
                      screenResult.matchScore >= 70
                        ? "#2ecc71"
                        : screenResult.matchScore >= 50
                        ? "#f39c12"
                        : "#e74c3c",
                  }}
                >
                  {screenResult.matchScore}%
                </div>
                <div className={styles.scoreLabel}>Match Score</div>
                <div
                  className={styles.recommendation}
                  style={{
                    background:
                      screenResult.recommendation === "Shortlist"
                        ? "#d4edda"
                        : screenResult.recommendation === "Reject"
                        ? "#f8d7da"
                        : "#fff3cd",
                    color:
                      screenResult.recommendation === "Shortlist"
                        ? "#155724"
                        : screenResult.recommendation === "Reject"
                        ? "#721c24"
                        : "#856404",
                  }}
                >
                  {screenResult.recommendation}
                </div>
              </div>

              <div className={styles.lists}>
                <div className={styles.strengths}>
                  <h4>✅ Strengths</h4>
                  <ul>
                    {screenResult.strengths.map((s: string, i: number) => (
                      <li key={i}>{s}</li>
                    ))}
                  </ul>
                </div>

                <div className={styles.gaps}>
                  <h4>❌ Gaps</h4>
                  <ul>
                    {screenResult.gaps.map((g: string, i: number) => (
                      <li key={i}>{g}</li>
                    ))}
                  </ul>
                </div>
              </div>
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default AIDashboard;
