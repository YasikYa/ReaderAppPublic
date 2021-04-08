import { Link, Redirect } from 'react-router-dom';
import { paths } from 'routes/paths';
import { useDispatch, useSelector } from 'store';
import { PageType } from 'pages/types';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import { REGEXP_EMAIL } from 'shared/constants';
import { fetchLogin } from 'store/auth/actions';

const useStyles = makeStyles((theme) => ({
    paper: {
        paddingTop: theme.spacing(10),
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
    },
    avatar: {
        margin: theme.spacing(1),
        backgroundColor: theme.palette.secondary.main,
    },
    form: {
        marginTop: theme.spacing(1),
    },
    submit: {
        margin: theme.spacing(3, 0, 2),
    },
}));

type LoginForm = {
    email: string;
    password: string;
};

const LoginFormSchema = Yup.object().shape({
    email: Yup.string().required('Email required').matches(REGEXP_EMAIL, 'Invalid email address'),
    password: Yup.string().required('Password required'),
});

const LoginPage: PageType = ({ className }) => {
    const dispatch = useDispatch();
    const isAuthorized = useSelector((state) => state.auth.isAuthorized);

    const classes = useStyles();

    const form = useFormik<LoginForm>({
        initialValues: {
            email: '',
            password: '',
        },
        onSubmit: async (values) => {
            await dispatch(fetchLogin(values));
        },
        validationSchema: LoginFormSchema,
    });

    if (isAuthorized) {
        return <Redirect to={paths.HOME} />;
    }

    return (
        <Container component="main" maxWidth="xs">
            <div className={classes.paper}>
                <Avatar className={classes.avatar}>
                    <LockOutlinedIcon />
                </Avatar>

                <Typography component="h1" variant="h5">
                    Log in
                </Typography>

                <form className={classes.form} noValidate onSubmit={form.handleSubmit}>
                    <TextField
                        onChange={form.handleChange}
                        onBlur={form.handleBlur}
                        value={form.values.email}
                        error={form.touched.email && Boolean(form.errors.email)}
                        helperText={form.touched.email && form.errors.email}
                        variant="outlined"
                        margin="normal"
                        fullWidth
                        id="email"
                        label="Email Address"
                        name="email"
                    />
                    
                    <TextField
                        onChange={form.handleChange}
                        onBlur={form.handleBlur}
                        value={form.values.password}
                        error={form.touched.password && Boolean(form.errors.password)}
                        helperText={form.touched.password && form.errors.password}
                        variant="outlined"
                        margin="normal"
                        fullWidth
                        name="password"
                        label="Password"
                        type="password"
                    />

                    <Button
                        disabled={!form.isValid}
                        type="submit"
                        fullWidth
                        size="large"
                        variant="contained"
                        color="primary"
                        className={classes.submit}
                    >
                        Log In
                    </Button>

                    <Link to={paths.SIGNUP}>Don't have an account? Sign Up</Link>
                </form>
            </div>
        </Container>
    );
};

export default LoginPage;
