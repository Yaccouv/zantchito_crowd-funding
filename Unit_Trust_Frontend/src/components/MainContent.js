import React from 'react';
import "./../styles/common.css";

const MainContent = ({ title, children }) => {
  return (
    <div className="main-content">
      <h1>{title}</h1>
      {children}
    </div>
  );
};

export default MainContent;
