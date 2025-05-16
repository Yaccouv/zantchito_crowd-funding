import React, { useState, useEffect } from "react";
import axios from "axios";
import "../../styles/Investor/InvestorTable.css"; // Import the CSS file

const InvestorTable = () => {
  const [investments, setInvestments] = useState([]);
  const [error] = useState(null);

  const fetchInvestments = async () => {
    try {
      const response = await axios.get(
        "https://localhost:7182/api/investor/getInvestments"
      );
      setInvestments(response.data.length > 0 ? response.data : []);
    } catch (error) {
      console.error("Error fetching investments:", error);
    setInvestments([]);
    }
  };
  useEffect(() => {
    fetchInvestments();
  }, []);

  if (error) return <div>{error}</div>;

  return (
    <div className="investment-table-container">
      <h2>Your Investments</h2>
      <button className="addApp"><a href="/investor">Add Applicant</a></button>
      <table className="investment-table">
        <thead>
          <tr>
            <th>Investor Number</th>
            <th>Full Name</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {investments.map((investment, index) => (
            <tr key={index}>
              <td>{investment.investorNumber}</td>
              <td>
                {investment.firstname} {investment.lastname}
              </td>
              <td>
                <div className="action-btn">
                  <a href="/signin" className="action-link">
                    Deposit
                  </a>
                  <a href="/signin" className="action-link">
                    Withdraw
                  </a>
                  <a href="/signin" className="action-link">
                    Balance
                  </a>
                  <a href="/signin" className="action-link">
                    View
                  </a>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default InvestorTable;
