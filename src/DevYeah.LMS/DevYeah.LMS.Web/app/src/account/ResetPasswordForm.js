import React from 'react';
import PropTypes from 'prop-types';
import { Formik } from 'formik';
import * as yup from 'yup';
import { getClassName } from '../common/utilities';
import './account.css';

const resetPasswordValidation = yup.object().shape({
  //todo:define a regular expression to validate the specific form that password required
  password: yup.string()
    .min(8, 'Too Short!')
    .max(16, 'Too Long!')
    .required('Required!'),
  confirmPassword: yup.string()
    .oneOf([yup.ref('password')], 'Must match the previous entry')
    .required('Password confirm is required!'),
});

export default function ResetPasswordForm({ submitHandler }) {
  return (
    <Formik
      initialValues={{password:'', confirmPassword:''}}
      validationSchema={resetPasswordValidation}
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
            <div>
              <h1 id="setNewPwd" className="h4">Reset password</h1>
              <p id="setNewPwdTips">
                Your password needs to have at least one symbol or number, 
                and have at least 8 characters.
              </p>
            </div>
            <div className="form-group font-weight-bold">
              <label id="passwordInput" htmlFor="password">Password</label>
              <input 
                type="password"
                className={getClassName(errors.password, touched.password)}
                id="password"
                value={values.password}
                onChange={handleChange}
                onBlur={handleBlur}
              />
              {errors.password 
                && <div id="passwordError" className="invalid-feedback">{errors.password}</div>}
            </div>
            <div className="form-group font-weight-bold">
              <label id="confirmPasswordInput" htmlFor="confirmPassword">Confirm Password</label>
              <input  
                type="password" 
                className={getClassName(errors.confirmPassword, touched.confirmPassword)}
                id="confirmPassword"
                value={values.confirmPassword}
                onChange={handleChange}
                onBlur={handleBlur}
              />
              {errors.confirmPassword 
                && <div id="confirmPasswordError" className="d-block invalid-feedback">{errors.confirmPassword}</div>}
            </div>
            <button
                type="submit" 
                className="btn btn-danger font-weight-bold" 
                id="resetPasswordBtn"
                disabled={isSubmitting}
            >
              Submit
            </button>
          </form>
        </div>
      )}
    </Formik>
  );
}

ResetPasswordForm.propTypes = {
  submitHandler: PropTypes.func.isRequired
}