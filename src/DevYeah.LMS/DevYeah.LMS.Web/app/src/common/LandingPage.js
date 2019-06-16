import React from 'react';
import { Link } from 'react-router-dom';
import './common.css';
import structuredImg from '../images/feature-structured.svg';
import practiceImg from '../images/feature-practice.svg';
import skillImg from '../images/feature-skill.svg';
import touchImg from '../images/feature-touch.svg';

export default function() {
  return (
    <div className="row align-items-center h-100">
      <div className="col-8 mx-auto">
        <div className="card h-100 justify-content-center">
          <h2 className="display-4 text-center font-weight-bold box-title">
            Get where you’re going faster with Dev Yeah!
          </h2>
          <ul className="text-left d-flex flex-row flex-wrap justify-content-between features-clear-style">
            <li className="d-flex flex-row features-clear-style features-item justify-content-between">
              <div>
                <img alt="features" src={structuredImg} />
              </div>
              <div className="ml-3">
                <h3 className="h2 title-underline">
                  Structured Curriculum
                </h3>
                <p className="features-font">
                  Our courses are designed to keep you on track, so you learn to code “today” not “someday.”
                </p>
              </div>
            </li>
            <li className="d-flex flex-row features-clear-style features-item justify-content-between">
              <div>
                <img alt="features" src={practiceImg} />
              </div>
              <div className="ml-3">
                <h3 className="h2 title-underline">
                  Practice Smarter
                </h3>
                <p className="features-font">
                  Drill the material with 85 coding quizzes and feel comfortable and confident.
                </p>
              </div>
            </li>
            <li className="d-flex flex-row features-clear-style features-item justify-content-between">
              <div>
                <img alt="features" src={skillImg} />
              </div>
              <div className="ml-3">
                <h3 className="h2 title-underline">
                  One Day, One New Skill
                </h3>
                <p className="features-font">
                  Most of our free courses take fewer than 11 hours.
                </p>
              </div>
            </li>
            <li className="d-flex flex-row features-clear-style features-item justify-content-between">
              <div>
                <img alt="features" src={touchImg} />
              </div>
              <div className="ml-3">
                <h3 className="h2 title-underline">
                  The Human Touch
                </h3>
                <p className="features-font">
                  Our global community of coaches, advisors, and graduates means there’s always someone to answer your question.
                </p>
              </div>
            </li>
          </ul>
          <span className="text-center">
            <Link
              to="/signup"
              className="btn btn-success btn-lg oval-btn"
            >
              Get Started
            </Link>
          </span>
        </div>
      </div>
    </div>
  );
}