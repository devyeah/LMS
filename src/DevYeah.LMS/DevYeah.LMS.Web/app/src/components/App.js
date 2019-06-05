import React from 'react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import SignUp from './SignUp';
import SignIn from './SignIn';
import Navbar from './Navbar';
import PageBuilding from './PageBuilding';
import '../index.css';

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
            <Route path="/signup" component={SignUp} />
            <Route path="/signin" component={SignIn} />
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
