import React from 'react';
import PropTypes from 'prop-types';
import { Formik } from 'formik';
import { Link } from 'react-router-dom';
import * as yup from 'yup';
import { getClassName } from '../common/utilities';
import './account.css';

const resetPasswordValidation = yup.object().shape({
  email: yup.string()
    .email('Invalid email address')
    .required('Required!'),
});

export default function ResetPasswordForm({ submitHandler }) {
  return (
    <Formik 
      initialValues={{email:''}}
      validationSchema={ resetPasswordValidation }
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
          <form className="bg-white mb-4" onSubmit={handleSubmit}>
            <div className="mb-4">
              <h1 id="resetPwd" className="h4 mb-4">Reset password</h1>
              <p id="resetPwdTips">
                Enter the email address associated with your account, 
                and we’ll email you a link to reset your password.
              </p>
            </div>
            <div className="form-group font-weight-bold mb-4">
              <label id="emailInput" htmlFor="email">Email address</label>
              <input 
                type="email" 
                className={getClassName(errors.email, touched.email)}
                id="email"
                value={values.email}
                onChange={handleChange}
                onBlur={handleBlur}
              />
              {errors.email 
                && <div id="emailError" className="d-block invalid-feedback">{errors.email}</div>}
            </div>
            <div className="form-group font-weight-bold">
              <Link 
                to="/signIn"
                id="backToSignInBtn"
              >
              &#60; Back to Sign In
              </Link>
              <button
                type="submit" 
                className="btn btn-danger float-sm-right font-weight-bold" 
                id="resetPasswordBtn"
                disabled={isSubmitting}
              >
                Send Reset Link
              </button>
            </div>
          </form>
        </div>
      )}
    </Formik>
  );
}

ResetPasswordForm.propTypes = {
  submitHandler: PropTypes.func.isRequired
}