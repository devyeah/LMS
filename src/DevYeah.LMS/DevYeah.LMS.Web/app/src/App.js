import React from 'react';
import SignUpPage from './features/identity/SignUpPage';
import Navbar from './features/common/Navbar';
import './index.css';

function App() {
  return (
    <div>
      <header>
        <Navbar />
      </header>
      <main role="main">
        <SignUpPage />
      </main>
      <footer className="footer text-center">
        <div className="container">
          <span id="copyright" className="text-muted">
            Dev Yean! &copy; {new Date().getFullYear()}
          </span>
        </div>
      </footer>
    </div>
  );
}

export default App;
