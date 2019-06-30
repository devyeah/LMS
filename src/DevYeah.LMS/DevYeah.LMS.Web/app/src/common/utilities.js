export const getClassName = (error, touched) => {
  let name = 'form-control';
  if (error && touched) name += ' is-invalid';
  return name;
}