import React from "react";
import { Link, useNavigate } from "react-router-dom";
import "./../styles/common.css";

const Navbar = () => {
  const navigate = useNavigate();
  const role = localStorage.getItem("role"); // Get user role

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("role"); // Clear stored role
    navigate("/signin");
  };

  return (
    <div className="sidebar">
      <h2>Zantchito Skills</h2>
      <ul>
        {/* Links for Entrepreneurs / Investors */}
        {role === "Investor" && (
          <>
            <li className="section-title">📊 Dashboard</li>
            <li>
              <Link to="/Business">➕ Create a Campaign</Link>
            </li>
            <li>
              <Link to="/CampaignCard">📁 Campaigns</Link>
            </li>

            <li className="section-title">💬 Communication</li>
            <li>
              <Link to="/Chat">💬 Chat</Link>
            </li>
            <li>
              <Link to="/Notifications">🔔 Notifications</Link>
            </li>

            <li className="section-title">⚙️ Account</li>
            <li>
              <Link to="/Settings">⚙️ Settings</Link>
            </li>
          </>
        )}

        {/* Links for Staff */}
        {role === "Staff" && (
          <>
            <li className="section-title">🧑‍💼 Staff Panel</li>
            <li>
              <Link to="/ApplicantList">🆕 New Entrepreneurs</Link>
            </li>
            <li>
              <Link to="/CampaignCard">📁 All Campaigns</Link>
            </li>
            <li>
              <Link to="/Approvals">✅ Approve Campaigns</Link>
            </li>

            <li className="section-title">📄 Reports & Communication</li>
            <li>
              <Link to="/Report">📝 Reports</Link>
            </li>
            <li>
              <Link to="/Chat">💬 Chat</Link>
            </li>

            <li className="section-title">⚙️ Account</li>
          </>
        )}

        <li>
          <button onClick={handleLogout} className="logout-button">
          🚪Logout
          </button>
        </li>
      </ul>
    </div>
  );
};

export default Navbar;
