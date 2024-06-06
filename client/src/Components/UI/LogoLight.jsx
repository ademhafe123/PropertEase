import React from "react";

import "./Logo.css";
import logoLight from "../../images/Logo-light.png";

const LogoLight = () => {
  return (
    <div className="logo-container">
      <img src={logoLight} className="logo-img" />
    </div>
  );
};

export default LogoLight;
