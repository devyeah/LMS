import React from 'react';
import PropTypes from 'prop-types';
import { Formik } from 'formik';
import { Link } from 'react-router-dom';
import * as yup from 'yup';
import { getClassName } from '../common/utilities';

const signInValidation = yup.object().shape({
  email: yup.string()
    .email('Invalid email address')
    .required('Required!'),
  password: yup.string()
    .min(8, 'Too Short!')
    .max(16, 'Too Long!')
    .required('Required'),
});

export default function SignInForm({ submitHandler }) {
  return (
    <Formik
      initialValues={{email: '', password: ''}}
      validationSchema={signInValidation}
      onSubmit={submitHandler}
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
            <div className="text-center mb-4">
              <h1 id="title" className="h2 mb-4">Dev Yeah!</h1>
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
            <button
              type="submit"
              className="btn btn-danger btn-block font-weight-bold"
              id="signinBtn"
              disabled={isSubmitting}
            >
              Sign In
            </button>
            <hr />
            <span id="passwordForget">
              <Link 
                to="/forgetPassword"
              >
                Forget Password?
              </Link>
            </span>
          </form>
        </div>
      )}
    </Formik>
  );
}

SignInForm.propTypes = {
  submitHandler: PropTypes.func.isRequired
}