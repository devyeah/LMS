import React from 'react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import SignUpPage from './features/identity/SignUpPage';
import Navbar from './features/common/Navbar';
import PageBuilding from './features/common/PageBuilding';
import './index.css';

function App() {
  return (
    <Router>
      <div>
        <header>
          <Navbar />
        </header>
        <main role="main">
          <Switch>
            <Route path="/" exact component={PageBuilding} />
            <Route path="/signup" component={SignUpPage} />
          </Switch>
        </main>
        <footer className="footer text-center">
          <div className="container">
            <span id="copyright" className="text-muted">
              Dev Yean! &copy; {new Date().getFullYear()}
            </span>
          </div>
        </footer>
      </div>
    </Router>
  );
}

export default App;
