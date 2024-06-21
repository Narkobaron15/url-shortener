import './css/LoginRegisterPage.css'
import http_common from "../../common/http_common.ts";
import {ErrorMessage, Field, Form, Formik, FormikHelpers } from "formik";
import {loginValidationSchema} from "./schemas/Schemas.ts";
import {HiOutlineMail, HiOutlineLockClosed} from "react-icons/hi";
import LoginModel from "../../models/LoginModel.ts";
import {useNavigate} from "react-router-dom";

const initialValues = {
    email: '',
    password: '',
};
export default function LoginPage() {
    const navigate = useNavigate();

    const handleSubmit = async (
        values: LoginModel,
        { setSubmitting }: FormikHelpers<LoginModel>
    ) => {
        try {
            const response = await http_common.post('/user/login', values);
            console.log(response.data.message);
        } catch (error) {
            console.error('Error logging in', error);
        } finally {
            setSubmitting(false);
            navigate('/');
        }
    };

    return (
        <div className="wrapper">
            <h2>Login</h2>
            <Formik initialValues={initialValues}
                    validationSchema={loginValidationSchema}
                    onSubmit={handleSubmit}>
                {({ isSubmitting }) => (
                    <Form className="form">
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
                                {isSubmitting ? 'Logging in...' : 'Login'}
                            </button>
                        </div>
                    </Form>
                )}
            </Formik>
        </div>
    );
}