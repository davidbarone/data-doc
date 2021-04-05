import { h } from "preact";
import { Router } from "preact-router";

import Header from "./header";

// Code-splitting is automated for `routes` directory
import Home from "../routes/home";
import Profile from "../routes/profile";
import Projects from "../routes/projects";
import Project from "../routes/project";
import Entity from "../routes/entity";
import Attribute from "../routes/attribute";
import { MyToastContainer } from "../components/myToast";

const App = () => (
  <div id="app">
    <MyToastContainer />
    <Header />
    <Router>
      <Home path="/" />
      <Profile path="/profile/" user="me" />
      <Profile path="/profile/:user" />
      <Projects path="/projects/" />
      <Project path="/project/:projectId/:index?" />
      <Entity path="/entity/:projectId/:entityName/:index?" />
      <Attribute path="/attribute/:projectId/:entityName/:attributeName" />
    </Router>
  </div>
);

export default App;
