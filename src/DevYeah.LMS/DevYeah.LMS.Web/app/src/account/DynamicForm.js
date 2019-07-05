import React from 'react';
import PropTypes from 'prop-types';
import { Formik } from 'formik';
import { Link } from 'react-router-dom';
import { getClassName } from '../common/utilities';
import CardHeader from './CardHeader';
import './account.css';

export default function DynamicForm({isEmbedded, formName, formValidation, formMeta, submitHandler, error, success}){
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
                  <CardHeader header={header} />
                }

                {error 
                  && 
                  <Alert 
                    message={error} 
                    type="error"
                  />
                }
                {success
                  &&
                  <Alert 
                    message={success}
                    type="success"
                  />
                }
                
                {elements.map((item, index) => {
                  if (item.element === 'select')
                    return (<SelectElement 
                              key={item.id} 
                              item={item} 
                              errors={errors} 
                              values={values} 
                              handleChange={handleChange} 
                              handleBlur={handleBlur} 
                              isEmbedded={isEmbedded} 
                            />);
                  if (item.element === 'input')
                    return (<InputElement 
                              key={item.id} 
                              item={item}
                              errors={errors} 
                              touched={touched} 
                              values={values} 
                              handleChange={handleChange} 
                              handleBlur={handleBlur} 
                              isEmbedded={isEmbedded} 
                            />);
                  if (item.element === 'textarea')
                    return (<TextareaElement 
                              key={item.id} 
                              item={item} 
                              errors={errors} 
                              touched={touched} 
                              values={values} 
                              handleChange={handleChange} 
                              handleBlur={handleBlur} 
                              isEmbedded={isEmbedded} 
                            />);
                  if (item.element === 'hidden')
                    return (<HiddenElement 
                              key={item.id} 
                              item={item} 
                              values={values} 
                              handleChange={handleChange} 
                            />);
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
                  <ExtraLinks 
                    links={extraLink}
                  />
                }
              </form>
            </div>
          </div>
        </div>
      )}
    </Formik>
  );
}

function Alert({message, type}) {
  const style = type === 'error' ? "alert alert-danger" : "alert alert-success";
  
  return (
    <div className={style} role="alert">
      {message}
      <button type="button" className="close" data-dismiss="alert" aria-label="Close">
        <span aria-hidden="true">&times;</span>
      </button>
    </div>
  );
}

function ExtraLinks({ links }) {
  return (
    <span>
      <hr />
      {links.map((item) => (
        <Link 
        key={item.id}
        to={item.target}
        className={item.style}
        >
          {item.text}
        </Link>
      ))}
    </span>
  );
}

function HiddenElement({ item, values, handleChange }) {
  return (
    <input 
      type="text" 
      value={values[item.id]} 
      onChange={handleChange}
      hidden 
    />
  );
}

function SelectElement({ item, errors, values, handleChange, handleBlur, isEmbedded }) {
  return (
    <div className={"form-group " + (isEmbedded ? "row" : "")}>
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

function InputElement({item, errors, touched, values, handleChange, handleBlur, isEmbedded}) {
  return (
    <div className={"form-group " + (isEmbedded ? "row" : "")}>
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

function TextareaElement({ item, errors, touched, values, handleChange, handleBlur, isEmbedded }) {
  return (
    <div className={"form-group " + (isEmbedded ? "row" : "")}>
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