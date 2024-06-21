import './css/LoginRegisterPage.css'
import http_common from "../../common/http_common.ts"
import {ErrorMessage, Field, Form, Formik, FormikHelpers} from "formik"
import {loginValidationSchema} from "./schemas/Schemas.ts"
import {HiOutlineLockClosed, HiOutlineUser} from "react-icons/hi"
import LoginModel from "../../models/LoginModel.ts"
import {Link, useNavigate} from "react-router-dom"
import {useState} from "react";

const initialValues = {
    username: '',
    password: '',
}
export default function LoginPage() {
    const navigate = useNavigate()
    const [showPassword, setShowPassword] = useState<boolean>(false)

    const handleSubmit = async (
        values: LoginModel,
        {setSubmitting}: FormikHelpers<LoginModel>
    ) => {
        try {
            const response = await http_common.post(
                '/user/login',
                values,
                {
                    withCredentials: false,
                }
            )
            console.log(response.data)

            localStorage.setItem('auth', "true")
            navigate('/')
        } catch (error) {
            console.error('Error logging in', error)
        } finally {
            setSubmitting(false)
        }
    }

    return (
        <div className="wrapper">
            <h2>Login</h2>
            <Formik initialValues={initialValues}
                    validationSchema={loginValidationSchema}
                    onSubmit={handleSubmit}>
                {({isSubmitting}) => (
                    <Form className="form">
                        <div className="mb-4">
                            <label htmlFor="username">
                                User name
                            </label>
                            <div className="relative">
                                <Field
                                    type="text"
                                    name="username"
                                    id="username"/>
                                <HiOutlineUser className="icon"/>
                            </div>
                            <ErrorMessage name="username" component="div" className="error"/>
                        </div>

                        <div className="mb-6">
                            <label htmlFor="password">
                                Password
                            </label>
                            <div className="relative">
                                <Field
                                    type={showPassword ? "text" : "password"}
                                    name="password"
                                    id="password"
                                    className="mb-3"/>
                                <HiOutlineLockClosed
                                    onClick={() => setShowPassword(!showPassword)}
                                    className="icon"/>
                            </div>
                            <ErrorMessage name="password" component="div" className="error"/>
                        </div>

                        <div className="flex items-center justify-between">
                            <button
                                type="submit"
                                className="submit-btn"
                                disabled={isSubmitting}>
                                {isSubmitting ? 'Logging in...' : 'Login'}
                            </button>
                        </div>
                        <p className="mt-2">
                            Don't have an account?&nbsp;
                            <Link to="/register" className="link">Register</Link>
                        </p>
                    </Form>
                )}
            </Formik>
        </div>
    )
}
