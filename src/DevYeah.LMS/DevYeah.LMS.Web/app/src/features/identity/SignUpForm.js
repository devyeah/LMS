import React from 'react';
import PropTypes from 'prop-types';
import { Formik } from 'formik';
import * as yup from 'yup';

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
    .required('Required'),
  userType: yup.string()
    .required('Required!'),
});

function getClassName(error, touched){
  let name = 'form-control';
  if (error && touched) name += ' is-invalid';
  return name;
}

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
        <div className="card form-signup">
        <form className="bg-white mb-4" onSubmit={handleSubmit}>
          <div className="text-center mb-4">
            <h1 className="h2 mb-4">Dev Yeah!</h1>
            <p className="mb-3 font-weight-bold">Start learning with Dev Yeah!</p>
          </div>
          <div className="form-group">
            <label htmlFor="username">Username</label>
            <input
              type="text"
              className={getClassName(errors.username, touched.username)}
              id="username"
              value={values.username}
              placeholder="Enter Username"
              onChange={handleChange}
              onBlur={handleBlur}
            />            
            {errors.username && <div className="invalid-feedback">{errors.username}</div>}
          </div>
          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input
              type="email"
              className={getClassName(errors.email, touched.email)}
              id="email"
              value={values.email}
              placeholder="Email Address"
              onChange={handleChange}
              onBlur={handleBlur}
            />
            {errors.email && <div className="invalid-feedback">{errors.email}</div>}
          </div>
          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input
              type="password"
              className={getClassName(errors.password, touched.password)}
              id="password"
              value={values.password}
              placeholder="Password"
              onChange={handleChange}
              onBlur={handleBlur}
            />
            {errors.password && <div className="invalid-feedback">{errors.password}</div>}
          </div>
          <div className="form-group">
            <label htmlFor="userType">User Type</label>
            <select 
              className="form-control" id="userType" 
              value={values.userType} 
              onChange={handleChange} 
              onBlur={handleBlur}
            >
              <option value="">Select a type</option>
              <option value="1">Student</option>
              <option value="2">Teacher</option>
            </select>
            {errors.userType && <div className="d-block invalid-feedback">{errors.userType}</div>}
          </div>
          <button type="submit" className="btn btn-primary btn-block" disabled={isSubmitting}>Sign Up</button>    
        </form>
        </div>
      )}
    </Formik>
  );
}

SignUpForm.propTypes = {
  submitHandler: PropTypes.func.isRequired
}