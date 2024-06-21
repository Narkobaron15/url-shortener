import './css/LoginRegisterPage.css'
import http_common from "../../common/http_common.ts"
import RegisterModel from "../../models/RegisterModel.ts"
import {ErrorMessage, Field, Form, Formik, FormikHelpers} from "formik"
import {HiOutlineLockClosed, HiOutlineUser, HiOutlineMail} from "react-icons/hi"
import {registerValidationSchema} from "./schemas/Schemas.ts"
import {useNavigate} from "react-router-dom"

const initialValues = {
    username: '',
    email: '',
    password: '',
}

export default function RegisterPage() {
    const navigate = useNavigate()

    const handleSubmit = async (
        values: RegisterModel,
        { setSubmitting } : FormikHelpers<RegisterModel>
    ) => {
        try {
            let response = await http_common.post('/user/register', values)
            console.log(response.data.message)
            response = await http_common.post('/user/login', values)
            console.log(response.data.message)
            navigate('/')
        } catch (error) {
            console.error('Error registering', error)
        } finally {
            setSubmitting(false)
        }
    }
    
    return (
        <div className="wrapper">
            <h2>Register</h2>
            <Formik initialValues={initialValues}
                    validationSchema={registerValidationSchema}
                    onSubmit={handleSubmit}>
                {({ isSubmitting }) => (
                    <Form className="form">
                        <div className="mb-4">
                            <label htmlFor="username">
                                Username
                            </label>
                            <div className="relative">
                                <Field
                                    type="text"
                                    name="username"
                                    id="username"/>
                                <HiOutlineUser className="icon" />
                            </div>
                            <ErrorMessage name="username" component="div" className="error" />
                        </div>

                        <div className="mb-4">
                            <label htmlFor="email">
                                Email
                            </label>
                            <div className="relative">
                                <Field
                                    type="email"
                                    name="email"
                                    id="email"/>
                                <HiOutlineMail className="icon" />
                            </div>
                            <ErrorMessage name="email" component="div" className="error" />
                        </div>

                        <div className="mb-6">
                            <label htmlFor="password">
                                Password
                            </label>
                            <div className="relative">
                                <Field
                                    type="password"
                                    name="password"
                                    id="password"
                                    className="mb-3"/>
                                <HiOutlineLockClosed className="icon" />
                            </div>
                            <ErrorMessage name="password" component="div" className="error" />
                        </div>

                        <div className="flex items-center justify-between">
                            <button
                                type="submit"
                                className="submit-btn"
                                disabled={isSubmitting}>
                                {isSubmitting ? 'Registering...' : 'Registered'}
                            </button>
                        </div>
                    </Form>
                )}
            </Formik>
        </div>
    )
}