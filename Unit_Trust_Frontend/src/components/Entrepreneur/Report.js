import React, { useEffect, useState } from "react";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  PieChart,
  Pie,
  Cell,
  Legend,
  ResponsiveContainer
} from "recharts";
import axios from "axios";
import Navbar from "../Navbar";
import Footer from "../Footer";
import "../../styles/common.css";

const COLORS = ['#00C49F', '#FF8042']; // Success, Failure

const ReportPage = () => {
  const [campaignData, setCampaignData] = useState([]);
  const [paymentSummary, setPaymentSummary] = useState([]);
  const [evaluatedCampaigns, setEvaluatedCampaigns] = useState([]);

  useEffect(() => {
    fetchReportData();
  }, []);

  const fetchReportData = async () => {
    try {
      const campaignResponse = await axios.get("https://localhost:7182/api/report/campaign-funding");
      const status3CampaignsResponse = await axios.get("https://localhost:7182/api/report/payment-status-summary");

      setCampaignData(campaignResponse.data);

      const campaigns = status3CampaignsResponse.data;
      let successCount = 0;
      let failureCount = 0;

      const evaluated = campaigns.map(campaign => {
        const createdAt = new Date(campaign.createdAt);
        const endDate = new Date(campaign.endDate);
        const raisedAmount = campaign.raisedAmount || 0;
        const fundingGoal = campaign.fundingGoal || 0;

        // Calculate the project duration in days
        const durationInDays = Math.floor((endDate - createdAt) / (1000 * 60 * 60 * 24));

        // Define success/failure criteria
        const isSuccess = raisedAmount >= fundingGoal && durationInDays <= 30;
        
        // Count successes and failures
        if (isSuccess) successCount++;
        else failureCount++;

        return {
          projectTitle: campaign.projectTitle,
          raisedAmount,
          fundingGoal,
          durationInDays,
          status: isSuccess ? "Success" : "Failure"
        };
      });

      const total = successCount + failureCount;
      const summary = [
        {
          status: 'Success (≤30 days + Funded) ',
          count: successCount,
          percentage: ((successCount / total) * 100).toFixed(1)
        },
        {
          status: 'Failure (>30 days or Underfunded) ',
          count: failureCount,
          percentage: ((failureCount / total) * 100).toFixed(1)
        }
      ];

      setPaymentSummary(summary);
      setEvaluatedCampaigns(evaluated);
    } catch (error) {
      console.error("Error fetching report data", error);
    }
  };

  return (
    <>
      <div className="dashboard-container">
        <Navbar />
        <div className="report-container">
          <h2>Total Amount Raised Per Entrepreneur</h2>
          <ResponsiveContainer width="100%" height={400}>
            <BarChart data={campaignData} margin={{ top: 20, right: 30, left: 20, bottom: 5 }}>
              <XAxis dataKey="projectTitle" />
              <YAxis />
              <Tooltip />
              <Bar dataKey="raisedAmount" fill="#8884d8" />
            </BarChart>
          </ResponsiveContainer>

          <h2 style={{ marginTop: "3rem" }}>Project Success/Failure Summary</h2>
          <ResponsiveContainer width="100%" height={400}>
            <PieChart>
              <Pie
                data={paymentSummary}
                dataKey="count"
                nameKey="status"
                cx="50%"
                cy="50%"
                outerRadius={100}
                label={({ name, percent }) => `${(percent * 100).toFixed(1)}%`}
              >
                {paymentSummary.map((entry, index) => (
                  <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                ))}
              </Pie>
              <Legend />
              <Tooltip formatter={(value, name, props) => [`${value} Campaigns`, props.payload.status]} />
            </PieChart>
          </ResponsiveContainer>

          <h2 style={{ marginTop: "3rem" }}>Project Details</h2>
          <div className="table-responsive">
            <table className="report-table">
              <thead>
                <tr>
                  <th>Project Title</th>
                  <th>Raised Amount (MWK)</th>
                  <th>Funding Goal (MWK)</th>
                  <th>Status</th>
                  <th>Days</th>
                </tr>
              </thead>
              <tbody>
                {evaluatedCampaigns.map((campaign, index) => (
                  <tr key={index}>
                    <td>{campaign.projectTitle}</td>
                    <td>{campaign.raisedAmount.toLocaleString()}</td>
                    <td>{campaign.fundingGoal.toLocaleString()}</td>
                    <td style={{ color: campaign.status === "Success" ? "green" : "red" }}>
                      {campaign.status}
                    </td>
                    <td>{campaign.durationInDays}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
      <Footer />
    </>
  );
};

export default ReportPage;
