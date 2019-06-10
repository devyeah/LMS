import React from 'react';
import PropTypes from 'prop-types';
import { Formik } from 'formik';
import { Link } from 'react-router-dom';
import { getClassName } from '../common/utilities';
import './account.css';

export default function DynamicForm({formName, formValidation, formMeta, submitHandler}){
  const { initialValues, header, elements, button, extraLink } = formMeta;
  return (
    <Formik
      initialValues={initialValues} 
      validationSchema={formValidation} 
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
          <form id={formName} className="bg-white mb-4 font-weight-bold" onSubmit={handleSubmit}>
            <div className={header.alignStyle}>
              <h1 id={header.id} className={header.style}>{header.text}</h1>
              {header.tips && <p className={header.tips.style} id={header.tips.id}>{header.tips.message}</p>}
            </div>
            {elements.map((item, index) => {
              if (item.element === 'select')
                return renderSelectElement(item, index, errors, touched, values, handleChange, handleBlur);
              else 
                return renderInputElement(item, index, errors, touched, values, handleChange, handleBlur);
            })}
            <button
              type="submit" 
              className={button.style} 
              id={button.id}
              disabled={isSubmitting}
            >
              {button.text}
            </button>
            {extraLink 
              && 
              <span id={extraLink.id}>
                <hr />
                <Link 
                  to={extraLink.target}
                >
                  {extraLink.text}
                </Link>
              </span>
            }
          </form>
        </div>
      )}
    </Formik>
  );
}

function renderSelectElement(item, key, errors, touched, values, handleChange, handleBlur) {
  return (
    <div key={key} className="form-group">
      <label id={`${item.id}Input`} htmlFor={item.id}>{item.label}</label>
      <item.element 
        className="form-control" 
        id={item.id} 
        value={values[item.id]} 
        onChange={handleChange} 
        onBlur={handleBlur}
      >
        {item.options.map((option, index) => <option key={index} value={option.value}>{option.text}</option>)}
      </item.element>
      {errors.userType 
        && <div id="userTypeError" className="d-block invalid-feedback">{errors.userType}</div>}
    </div>
  );
}

function renderInputElement(item, key, errors, touched, values, handleChange, handleBlur) {
  return (
    <div key={key} className="form-group">
      <label  id={`${item.id}Input`} htmlFor={item.id}>{item.label}</label>
      <item.element
        type={item.type}
        className={getClassName(errors[item.id], touched[item.id])}
        id={item.id}
        value={values[item.id]}
        placeholder={item.placeholder}
        onChange={handleChange}
        onBlur={handleBlur}
      />            
      {errors[item.id] 
        && <div id={`${item.id}Error`} className="invalid-feedback">{errors[item.id]}</div>}
    </div>
  );
}

DynamicForm.propTypes = {
  formValidation: PropTypes.object.isRequired,
  formMeta: PropTypes.object.isRequired,
  formName: PropTypes.string.isRequired,
  submitHandler: PropTypes.func.isRequired,
}