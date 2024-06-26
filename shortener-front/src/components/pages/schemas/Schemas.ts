import * as Yup from "yup"

const loginValidationSchema = Yup.object({
    username: Yup.string().required('Required'),
    password: Yup.string().min(8, 'Password must be at least 8 characters long')
        .matches(
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$/,
            `Password must contain at least one lowercase letter, 
            one uppercase letter, one digit, and one non-alphanumeric character`
        )
        .required('Password is required')
})

const registerValidationSchema = loginValidationSchema.concat(
    Yup.object({
        email: Yup.string().email('Invalid email address').required('Required')
    })
)

export {loginValidationSchema, registerValidationSchema}