import React from 'react';
import PropTypes from 'prop-types';
import { Formik } from 'formik';
import * as yup from 'yup';
import { getClassName } from '../common/utilities';

const signUpValidation = yup.object().shape({
  username: yup.string()
    .min(3, 'Too Short!')
    .max(50, 'Too Long!')
    .required('Required!'),
  email: yup.string()
    .email('Invalid email address')
    .required('Required!'),
  password: yup.string()
    .min(8, 'Too Short!')
    .max(16, 'Too Long!')
    .required('Required!'),
  userType: yup.string()
    .required('Required!'),
});

export default function SignUpForm(props) {
  return (
    <Formik
      initialValues={{ username: '', email: '', password: '', userType: '' }} 
      validationSchema={signUpValidation} 
      onSubmit={props.submitHandler}
    >
      {({
        values,
        errors,
        touched,
        handleChange,
        handleBlur,
        handleSubmit,
        isSubmitting,
      }) => (
        <div className="card form-center">
        <form className="bg-white mb-4 font-weight-bold" onSubmit={handleSubmit}>
          <div className="text-center">
            <h1 id="title" className="h2">Dev Yeah!</h1>
            <p id="subtitle">Start learning with Dev Yeah!</p>
          </div>
          <div className="form-group">
            <label  id="usernameInput" htmlFor="username">Username</label>
            <input
              type="text"
              className={getClassName(errors.username, touched.username)}
              id="username"
              value={values.username}
              placeholder="Enter Username"
              onChange={handleChange}
              onBlur={handleBlur}
            />            
            {errors.username 
              && <div id="usernameError" className="invalid-feedback">{errors.username}</div>}
          </div>
          <div className="form-group">
            <label id="emailInput" htmlFor="email">Email</label>
            <input
              type="email"
              className={getClassName(errors.email, touched.email)}
              id="email"
              value={values.email}
              placeholder="Email Address"
              onChange={handleChange}
              onBlur={handleBlur}
            />
            {errors.email 
              && <div id="emailError" className="invalid-feedback">{errors.email}</div>}
          </div>
          <div className="form-group">
            <label id="passwordInput" htmlFor="password">Password</label>
            <input
              type="password"
              className={getClassName(errors.password, touched.password)}
              id="password"
              value={values.password}
              placeholder="Password"
              onChange={handleChange}
              onBlur={handleBlur}
            />
            {errors.password 
              && <div id="passwordError" className="invalid-feedback">{errors.password}</div>}
          </div>
          <div className="form-group">
            <label id="userTypeInput" htmlFor="userType">User Type</label>
            <select 
              className="form-control" 
              id="userType" 
              value={values.userType} 
              onChange={handleChange} 
              onBlur={handleBlur}
            >
              <option value="">Select a type</option>
              <option value="1">Student</option>
              <option value="2">Teacher</option>
            </select>
            {errors.userType 
              && <div id="userTypeError" className="d-block invalid-feedback">{errors.userType}</div>}
          </div>
          <button 
            type="submit" 
            className="btn btn-danger btn-block font-weight-bold" 
            id="signupBtn"
            disabled={isSubmitting}
          >
            Sign Up
          </button>    
        </form>
        </div>
      )}
    </Formik>
  );
}

SignUpForm.propTypes = {
  submitHandler: PropTypes.func.isRequired
}