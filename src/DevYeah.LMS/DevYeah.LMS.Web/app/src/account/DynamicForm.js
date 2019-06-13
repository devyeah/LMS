import React from 'react';
import PropTypes from 'prop-types';
import { Formik } from 'formik';
import { Link } from 'react-router-dom';
import { getClassName } from '../common/utilities';
import './account.css';

export default function DynamicForm({isEmbedded, formName, formValidation, formMeta, submitHandler}){
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
        <div className={isEmbedded ? "" : "row align-items-center h-100"}>
          <div className={isEmbedded ? "" : "col-6 mx-auto"}>
            <div className={isEmbedded ? "" : "card form-center h-100 justify-content-center"}>
              <form 
                id={formName} 
                className={"bg-white mb-4 " + (isEmbedded ? "" : "font-weight-bold")} 
                onSubmit={handleSubmit}
              >
                {!isEmbedded 
                  && 
                  <div className={header.alignStyle}>
                    <h1 id={header.id} className={header.style}>{header.text}</h1>
                    {header.tips && <p className={header.tips.style} id={header.tips.id}>{header.tips.message}</p>}
                  </div>
                }
                
                {elements.map((item, index) => {
                  if (item.element === 'select')
                    return renderSelectElement(item, index, errors, touched, values, handleChange, handleBlur, isEmbedded);
                  if (item.element === 'input')
                    return renderInputElement(item, index, errors, touched, values, handleChange, handleBlur, isEmbedded);
                  if (item.element === 'textarea')
                    return renderTextareaElement(item, index, errors, touched, values, handleChange, handleBlur, isEmbedded);
                  if (item.element === 'hidden')
                    return renderHiddenElement(item, index, values, handleChange);
                  return null;
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
                    {extraLink.map((link, index) => (
                      renderLinkElement(link, index)
                    ))}
                  </span>
                }
              </form>
            </div>
          </div>
        </div>
      )}
    </Formik>
  );
}

function renderLinkElement(link, index) {
  return (
    <Link 
      key={index}
      to={link.target}
      className={link.style}
    >
      {link.text}
    </Link>
  );
}

function renderHiddenElement(item, key, values, handleChange) {
  return (
    <input 
      key={key} 
      type="text" 
      value={values[item.id]} 
      onChange={handleChange}
      hidden 
    />
  );
}

function renderSelectElement(item, key, errors, touched, values, handleChange, handleBlur, isEmbedded) {
  return (
    <div key={key} className={"form-group " + (isEmbedded ? "row" : "")}>
      <label 
        id={`${item.id}Input`} 
        htmlFor={item.id}
        className={isEmbedded ? "col-sm-2" : ""}
      >
        {item.label}
      </label>
      <div className={isEmbedded ? "col-sm-10" : ""}>
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
    </div>
  );
}

function renderInputElement(item, key, errors, touched, values, handleChange, handleBlur, isEmbedded) {
  return (
    <div key={key} className={"form-group " + (isEmbedded ? "row" : "")}>
      <label  
        id={`${item.id}Input`} 
        htmlFor={item.id}
        className={isEmbedded ? "col-sm-2" : ""}
      >
        {item.label}
      </label>
      <div className={isEmbedded ? "col-sm-10" : ""}>
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
    </div>
  );
}

function renderTextareaElement(item, key, errors, touched, values, handleChange, handleBlur, isEmbedded) {
  return (
    <div key={key} className={"form-group " + (isEmbedded ? "row" : "")}>
      <label  
        id={`${item.id}Input`} 
        htmlFor={item.id}
        className={isEmbedded ? "col-sm-2" : ""}
      >
        {item.label}
      </label>
      <div className={isEmbedded ? "col-sm-10" : ""}>
        <item.element
          className={getClassName(errors[item.id], touched[item.id])}
          id={item.id}
          rows={item.rows}
          value={values[item.id]}
          onChange={handleChange}
          onBlur={handleBlur}
        />      
        {errors[item.id] 
          && <div id={`${item.id}Error`} className="invalid-feedback">{errors[item.id]}</div>}  
      </div>
    </div>
  );
}

DynamicForm.propTypes = {
  formValidation: PropTypes.object.isRequired,
  formMeta: PropTypes.object.isRequired,
  formName: PropTypes.string.isRequired,
  submitHandler: PropTypes.func.isRequired,
}