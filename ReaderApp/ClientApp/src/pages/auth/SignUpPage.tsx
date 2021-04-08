import { PageType } from 'pages/types';
import { REGEXP_EMAIL } from 'shared/constants';
import { useDispatch, useSelector } from 'store';
import { fetchSignUp } from 'store/auth/actions';
import * as Yup from 'yup';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import AssignmentIndIcon from '@material-ui/icons/AssignmentInd';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import { useFormik } from 'formik';
import { Redirect } from 'react-router';
import { Link } from 'react-router-dom';
import { paths } from 'routes/paths';

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

type SignUpForm = {
    name: string;
    email: string;
    password: string;
};

const SignUpFormSchema = Yup.object().shape({
    name: Yup.string().required('Name required'),
    email: Yup.string().required('Email required').matches(REGEXP_EMAIL, 'Invalid email address'),
    password: Yup.string()
        .required('Password required')
        .min(6, 'The password must contain at least 6 characters'),
});

const SignUpPage: PageType = () => {
    const dispatch = useDispatch();

    const isAuthorized = useSelector((state) => state.auth.isAuthorized);

    const classes = useStyles();

    const form = useFormik<SignUpForm>({
        initialValues: {
            name: '',
            email: '',
            password: '',
        },
        onSubmit: async (values) => {
            await dispatch(fetchSignUp(values));
        },
        validationSchema: SignUpFormSchema,
    });

    if (isAuthorized) {
        return <Redirect to={paths.HOME} />;
    }

    return (
        <Container component="main" maxWidth="xs">
            <div className={classes.paper}>
                <Avatar className={classes.avatar}>
                    <AssignmentIndIcon />
                </Avatar>

                <Typography component="h1" variant="h5">
                    Sign up
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
                        Sign up
                    </Button>

                    <Link to={paths.LOGIN}>Already have an account? Log in</Link>
                </form>
            </div>
        </Container>
    );
};

export default SignUpPage;
