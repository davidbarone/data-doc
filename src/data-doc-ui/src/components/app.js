import { h } from 'preact';
import { Router } from 'preact-router';

import Header from './header';

// Code-splitting is automated for `routes` directory
import Home from '../routes/home';
import Profile from '../routes/profile';
import Projects from '../routes/projects';
import Project from '../routes/project';

const App = () => (
	<div id="app">
		<Header />
		<Router>
			<Home path="/" />
			<Profile path="/profile/" user="me" />
			<Profile path="/profile/:user" />
			<Projects path="/projects/" />
			<Project path="/project/:projectId" />
		</Router>
	</div>
)

export default App;
