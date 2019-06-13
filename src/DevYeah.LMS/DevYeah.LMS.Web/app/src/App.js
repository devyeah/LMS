import React from 'react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import Navbar from './header/Navbar';
import SignUp from './account/SignUp';
import SignIn from './account/SignIn';
import ResetPassword from './account/ResetPassword';
import ForgetPassword from './account/ForgetPassword'; 
import PageBuilding from './common/PageBuilding';
import EditProfile from './account/EditProfileLayout';
import './index.css';

function App() {
  return (
    <Router>
      <div id="app">
        <header>
          <Navbar />
        </header>
        <main role="main" className="h-100">
          <Switch>
            <Route path="/" exact component={PageBuilding} />
            <Route path="/signup" component={SignUp} />
            <Route path="/signin" component={SignIn} />
            <Route path="/forgetPassword" component={ForgetPassword} />
            <Route path="/resetPassword" component={ResetPassword} />
            <Route path="/account/editProfile" component={EditProfile} />
          </Switch>
        </main>
        <footer className="footer text-center">
          <div className="container">
            <span id="copyright" className="text-muted">
              Dev Yean! &copy; 2018 - {new Date().getFullYear()}
            </span>
          </div>
        </footer>
      </div>
    </Router>
  );
}

export default App;
