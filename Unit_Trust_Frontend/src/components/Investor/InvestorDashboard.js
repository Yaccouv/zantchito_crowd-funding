import React from 'react';
import Navbar from '../Navbar';
import MainContent from '../MainContent';
import Footer from '../Footer';
import "../../styles/common.css";

const InvestorDashboard = () => {
  return (
    <div className="dashboard-container">
      {/* Sidebar (Navbar) */}
      <Navbar />

      {/* Main Content */}
      <MainContent title="Welcome to Zantchito Skills for Jobs">
        <p>
          Connect, empower, and grow with Zantchito. Submit ideas, track funding progress, and collaborate directly with entrepreneurs and donors.
        </p>
      </MainContent>

      {/* Footer */}
      <Footer />
    </div>
  );
};

export default InvestorDashboard;
