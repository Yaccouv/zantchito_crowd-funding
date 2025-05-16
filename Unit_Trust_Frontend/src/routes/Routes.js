import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Signin from '../components/Investor/Signin';  // Ensure this path is correct
import Signup from '../components/Investor/Signup';  // Ensure this path is correct
import Investor from '../components/Investor/Investor';
import Home from '../components/Investor/Home';
import InvestorTable from '../components/Investor/InvestorTable';
import InvestorDashboard from '../components/Investor/InvestorDashboard';
import StaffDashboard from '../components/Staff/StaffDashboard';
import ApplicantList from '../components/Staff/ApplicantList';
import Business from '../components/Entrepreneur/Business';
import CampaignCard from '../components/Entrepreneur/CampaignCard';
import Campaign from '../components/Entrepreneur/Campaign';
import CheckoutForm from '../components/stripe/CheckoutForm';
import Report from '../components/Entrepreneur/Report';



const AppRoutes = () => (
  <Router>
    <Routes>
      <Route path="/" element={<Home />} />  {/* Default route */}
      <Route path="/signin" element={<Signin />} />
      <Route path="/signup" element={<Signup />} />
      <Route path="/investor" element={<Investor />} />
      <Route path="/investorTable" element={<InvestorTable />} />
      <Route path="/investorDashboard" element={<InvestorDashboard />} />
      <Route path="/staffDashboard" element={<StaffDashboard />} />
      <Route path="/applicantList" element={<ApplicantList />} />
      <Route path="/Business" element={<Business />} />
      <Route path="/CampaignCard" element={<CampaignCard />} />
      <Route path="/Campaign" element={<Campaign />} />
      <Route path="/CheckoutForm" element={<CheckoutForm/>} />
      <Route path="/Report" element={<Report/>} />
    </Routes>
  </Router>
);

export default AppRoutes;
