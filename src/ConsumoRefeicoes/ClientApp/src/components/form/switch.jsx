import React from 'react';
import './form.css';

const Switch = ({
  label,
  field,
  children,
  onChange,
  form: { touched, errors },
  ...props
}) => {
  const campoComErro = touched[field.name] && errors[field.name];
  const classError = campoComErro ? 'label-error' : '';

  return (
    <div className="custom-control custom-switch">
      <input
        type="checkbox"
        className="custom-control-input"
        id={field.name}
        onChange={onChange}
        checked={field.value}
        {...field}
        {...props}
      />
      <label
        htmlFor={field.name}
        className={`custom-control-label ${classError}`}>
        {label}
      </label>
      {campoComErro ? (
        <div className="invalid-feedback">{errors[field.name]}</div>
      ) : null}
    </div>
  );
};

export default Switch;
