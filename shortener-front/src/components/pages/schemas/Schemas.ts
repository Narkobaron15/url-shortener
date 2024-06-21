import * as Yup from "yup";

const loginValidationSchema = Yup.object({
    email: Yup.string().email('Invalid email address').required('Required'),
    password: Yup.string().required('Required'),
});


const registerValidationSchema = Yup.object({
    username: Yup.string().required('Required'),
    email: Yup.string().email('Invalid email address').required('Required'),
    password: Yup.string()
        .min(8, 'Password must be at least 8 characters long')
        .matches(
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$/,
            `Password must contain at least one lowercase letter, 
            one uppercase letter, one digit, and one non-alphanumeric character`
        )
        .required('Password is required')
});

export { loginValidationSchema, registerValidationSchema }