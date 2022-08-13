using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CoreGraphics;
using Foundation;

using UIKit;
using WebKit;


namespace GamblingTracker.iOS
{
  public enum XAxisAnchor { Left, Center, Right }
  public enum YAxisAnchor { Top, Center, Bottom }

  public static class Extensions
  {
    public static System.Drawing.PointF ToPointF( this CoreGraphics.CGPoint cgPoint )
    {
      return new System.Drawing.PointF( (float)cgPoint.X, (float)cgPoint.Y );
    }

    public static CoreGraphics.CGPoint ToCGPoint( this System.Drawing.PointF point )
    {
      return new CoreGraphics.CGPoint( (nfloat)point.X, (nfloat)point.Y );
    }

    public static System.Drawing.RectangleF ToRectangleF( this CoreGraphics.CGRect cgRect )
    {
      return new System.Drawing.RectangleF( (float)cgRect.X, (float)cgRect.Y, (float)cgRect.Width, (float)cgRect.Height );
    }

    public static CoreGraphics.CGRect ToCGRect( this System.Drawing.RectangleF rect )
    {
      return new CoreGraphics.CGRect( (nfloat)rect.X, (nfloat)rect.Y, (nfloat)rect.Width, (nfloat)rect.Height );
    }


    public static System.Drawing.SizeF ToSizeF( this CoreGraphics.CGSize cgSize )
    {
      return new System.Drawing.SizeF( (float)cgSize.Width, (float)cgSize.Height );
    }

    public static CoreGraphics.CGSize ToCGSize( this System.Drawing.SizeF size )
    {
      return new CoreGraphics.CGSize( (nfloat)size.Width, (nfloat)size.Height );
    }
  }
  public static class ViewHelper
  {
    public static NSLayoutConstraint[] FillHeight( this UIView origin,
      UIView target, float margin = 0, bool activate = true )
      => origin.FillHeight( target, new UIEdgeInsets( margin, margin, margin, margin ), activate );

    public static NSLayoutConstraint[] FillWidth( this UIView origin,
      UIView target, float margin = 0, bool activate = true )
      => origin.FillWidth( target, new UIEdgeInsets( margin, margin, margin, margin ), activate );

    public static NSLayoutConstraint[] FillHeight( this UIView origin,
    UIView target, UIEdgeInsets edges, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.TopAnchor.ConstraintEqualTo(target.TopAnchor, edges.Top),
        origin.BottomAnchor.ConstraintEqualTo(target.BottomAnchor, -edges.Bottom)
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return contraints;
    }

    public static NSLayoutConstraint[] FillWidth( this UIView origin,
    UIView target, UIEdgeInsets edges, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.LeftAnchor.ConstraintEqualTo(target.LeftAnchor, edges.Left),
        origin.RightAnchor.ConstraintEqualTo(target.RightAnchor, -edges.Right)
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return contraints;
    }

    public static NSLayoutConstraint[] FullSizeOf( this UIView origin,
      UIView target, UIEdgeInsets edges, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.LeftAnchor.ConstraintEqualTo(target.LeftAnchor, edges.Left),
        origin.RightAnchor.ConstraintEqualTo(target.RightAnchor, -edges.Right),
        origin.TopAnchor.ConstraintEqualTo(target.TopAnchor, edges.Top),
        origin.BottomAnchor.ConstraintEqualTo(target.BottomAnchor, -edges.Bottom)
    };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return contraints;
    }

    public static NSLayoutConstraint[] WidthOf( this UIView origin,
      UIView target, float multiplier = 1f, float constant = 0f, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.WidthAnchor.ConstraintEqualTo(target.WidthAnchor, multiplier, constant )
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return contraints;
    }

    public static NSLayoutConstraint[] WidthOf( this UIView origin,
      NSLayoutDimension dimension, float multiplier = 1f, float constant = 0f, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.WidthAnchor.ConstraintEqualTo(dimension, multiplier, constant )
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return contraints;
    }

    public static NSLayoutConstraint[] HeightOf( this UIView origin,
      NSLayoutDimension dimension, float multiplier = 1f, float constant = 0f, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.HeightAnchor.ConstraintEqualTo(dimension, multiplier, constant )
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return contraints;
    }

    public static NSLayoutConstraint[] HeightOf( this UIView origin,
      UIView target, float multiplier = 1f, float constant = 0f, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.HeightAnchor.ConstraintEqualTo(target.HeightAnchor, multiplier, constant )
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return contraints;
    }

    public static NSLayoutConstraint[] Center( this UIView origin,
      UIView target, UIEdgeInsets edges, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.CenterXAnchor.ConstraintEqualTo(target.CenterXAnchor, edges.Left - edges.Right),
        origin.CenterYAnchor.ConstraintEqualTo(target.CenterYAnchor, edges.Top - edges.Bottom),
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return contraints;
    }

    public static NSLayoutConstraint[] CenterX( this UIView origin,
      UIView target, float offset = 0f, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.CenterXAnchor.ConstraintEqualTo(target.CenterXAnchor, offset ),
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return contraints;
    }

    public static NSLayoutConstraint[] CenterY( this UIView origin,
      UIView target, float offset = 0f, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.CenterYAnchor.ConstraintEqualTo(target.CenterYAnchor, offset),
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return contraints;
    }

    public static NSLayoutConstraint[] AlignCenterX( this UIView origin,
      NSLayoutXAxisAnchor targetAnchor, float constant = 0f, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.CenterXAnchor.ConstraintEqualTo( targetAnchor, constant),
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return contraints;
    }

    public static NSLayoutConstraint[] AlignCenterY( this UIView origin,
      NSLayoutYAxisAnchor targetAnchor, float constant = 0f, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.CenterYAnchor.ConstraintEqualTo(targetAnchor, constant),
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return contraints;
    }

    public static NSLayoutConstraint[] AlignLeft( this UIView origin,
      NSLayoutXAxisAnchor targetAnchor, float constant = 0f, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.LeftAnchor.ConstraintEqualTo(targetAnchor, constant),
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return contraints;
    }

    public static NSLayoutConstraint[] AlignRight( this UIView origin,
      NSLayoutXAxisAnchor targetAnchor, float constant = 0f, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.RightAnchor.ConstraintEqualTo(targetAnchor, constant),
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return contraints;
    }

    public static UIView AlignLeading( this UIView origin,
  NSLayoutXAxisAnchor targetAnchor, float constant = 0f, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.LeadingAnchor.ConstraintEqualTo(targetAnchor, constant),
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return origin;
    }

    public static UIView AlignTrailing( this UIView origin,
      NSLayoutXAxisAnchor targetAnchor, float constant = 0f, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.TrailingAnchor.ConstraintEqualTo(targetAnchor, constant),
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return origin;
    }

    public static UIView AlignTop( this UIView origin,
      NSLayoutYAxisAnchor targetAnchor, float constant = 0f, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.TopAnchor.ConstraintEqualTo(targetAnchor, constant),
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( contraints );

      return origin;
    }

    public static UIView AlignBottom( this UIView origin,
      NSLayoutYAxisAnchor targetAnchor, float constant = 0f, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var contraints = new[]
      {
        origin.BottomAnchor.ConstraintEqualTo(targetAnchor, constant),
      };

      if( activate )
      {
        NSLayoutConstraint.ActivateConstraints( contraints );
      }

      return origin;
    }

    public static NSLayoutConstraint[] Center( this UIView origin,
    UIView target, float margin = 0, bool activate = true )
    => origin.Center( target, new UIEdgeInsets( margin, margin, margin, margin ), activate );

    public static NSLayoutConstraint[] FullSizeOf( this UIView origin,
        UIView target, float margin = 0, bool activate = true )
        => origin.FullSizeOf( target, new UIEdgeInsets( margin, margin, margin, margin ), activate );

    public static UIView SetWidth( this UIView origin,
      nfloat width, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;

      // Try to replace existing constraint
      foreach( var constraint in origin.Constraints )
      {
        if( constraint.FirstItem == origin && constraint.FirstAttribute == NSLayoutAttribute.Width )
        {
          constraint.Constant = width;
          return origin;
        }
      }

      var Constraints = new[]
      {
        origin.WidthAnchor.ConstraintEqualTo( width ),
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( Constraints );

      return origin;
    }

    public static UIView SetHeight( this UIView origin,
      nfloat height, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;

      // Try to replace existing constraint
      foreach( var constraint in origin.Constraints )
      {
        if( constraint.FirstItem == origin && constraint.FirstAttribute == NSLayoutAttribute.Height )
        {
          constraint.Constant = height;
          return origin;
        }
      }

      var Constraints = new[]
      {
        origin.HeightAnchor.ConstraintEqualTo( height ),
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( Constraints );

      return origin;
    }

    public static UIView SetWidth( this UIView origin,
      NSLayoutDimension dimension, nfloat multiplier, nfloat constant = default( nfloat ), bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;

      var Constraints = new[]
      {
        origin.WidthAnchor.ConstraintEqualTo( dimension, multiplier, constant ),
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( Constraints );

      return origin;
    }

    public static UIView SetHeight( this UIView origin,
      NSLayoutDimension dimension, nfloat multiplier, nfloat constant = default( nfloat ), bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var Constraints = new[]
      {
        origin.HeightAnchor.ConstraintEqualTo( dimension, multiplier, constant ),
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( Constraints );

      return origin;
    }

    public static UIView SetSize( this UIView origin,
      nfloat width, nfloat height, bool activate = true )
    {
      origin.TranslatesAutoresizingMaskIntoConstraints = false;
      var Constraints = new[]
      {
        origin.WidthAnchor.ConstraintEqualTo( width ),
        origin.HeightAnchor.ConstraintEqualTo( height ),
      };

      if( activate )
        NSLayoutConstraint.ActivateConstraints( Constraints );

      return origin;
    }

    public static Dictionary<XAxisAnchor, NSLayoutXAxisAnchor> GetXAxisAnchors( this UIView origin )
    {
      return new Dictionary<XAxisAnchor, NSLayoutXAxisAnchor>()
      {
        { XAxisAnchor.Left, origin.LeftAnchor },
        { XAxisAnchor.Center, origin.CenterXAnchor },
        { XAxisAnchor.Right, origin.RightAnchor }
      };
    }
    public static Dictionary<YAxisAnchor, NSLayoutYAxisAnchor> GetYAxisAnchors( this UIView origin )
    {
      return new Dictionary<YAxisAnchor, NSLayoutYAxisAnchor>()
      {
        { YAxisAnchor.Top, origin.TopAnchor },
        { YAxisAnchor.Center, origin.CenterYAnchor },
        { YAxisAnchor.Bottom, origin.BottomAnchor }
      };
    }

    public static bool RemoveAllConstraints( this UIView view, NSLayoutAttribute attribute )
    {
      bool Removed = false;

      var isDimensionAttribute = attribute == NSLayoutAttribute.Width || attribute == NSLayoutAttribute.Height;
      var origin = isDimensionAttribute ? view : view.Superview;

      if( origin == null )
        return false;

      foreach( var constraint in origin.Constraints )
      {
        if( constraint.FirstItem == view && constraint.FirstAttribute == attribute )
        {
          origin.RemoveConstraint( constraint );
          Removed = true;
        }
      }

      if( !Removed && isDimensionAttribute && view.Superview != null )
      {
        origin = view.Superview;
        foreach( var constraint in origin.Constraints )
        {
          if( constraint.FirstItem == view && constraint.FirstAttribute == attribute )
          {
            origin.RemoveConstraint( constraint );
            Removed = true;
          }
        }
      }

      return Removed;
    }

    public static bool RemoveAllConstraints( this UIView origin, UIView view, NSLayoutAttribute attribute )
    {
      bool Removed = false;
      foreach( var constraint in origin.Constraints )
      {
        if( constraint.FirstItem == view && constraint.FirstAttribute == attribute )
        {
          origin.RemoveConstraint( constraint );
          Removed = true;
        }
      }
      return Removed;
    }

    public static bool RemoveAllConstraints( this UIView origin, UIView view )
    {
      bool Removed = false;
      foreach( var constraint in origin.Constraints )
      {
        if( constraint.FirstItem == view )
        {
          origin.RemoveConstraint( constraint );
          Removed = true;
        }
      }
      return Removed;
    }

    //public static UIEdgeInsets SafeAreaInsets
    //{
    //  get
    //  {
    //    if( iOSSoftwareVersion.iOS11 && UIApplication.SharedApplication != null && UIApplication.SharedApplication.KeyWindow != null )
    //    {
    //      if( NativePlatform.GetOSVersion() == 11 )
    //      {
    //        var insets = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets;
    //        insets.Top = 20;
    //        return insets;
    //      }
    //      else
    //      {
    //        return UIApplication.SharedApplication.KeyWindow.SafeAreaInsets;
    //      }
    //    }
    //    else
    //    {
    //      var insets = UIEdgeInsets.Zero;
    //      insets.Top = 20;
    //      return insets;
    //    }
    //  }
    //}

    public static void DisplayPrintDialog( string filename, UIPrintInfo printInfo = null, Action failure = null )
    {
      if( printInfo == null )
      {
        printInfo = UIPrintInfo.PrintInfo;
        printInfo.OutputType = UIPrintInfoOutputType.General;
      }

      var printer = UIPrintInteractionController.SharedPrintController;
      printer.PrintInfo = printInfo;
      printer.PrintingItem = NSData.FromFile( filename );
      printer.ShowsPageRange = true;

      printer.Present( true, ( handler, completed, err ) =>
      {
        if( !completed && err != null )
        {
          failure?.Invoke();
        }
      } );
    }

    //public static UIButton CreateVideoButton( bool darkBackground = false )
    //{
    //  var videoButton = UIButton.FromType( UIButtonType.Custom );
    //  videoButton.SetTitleColor( darkBackground ? UIColor.White : UIColor.Black, UIControlState.Normal );
    //  videoButton.BackgroundColor = darkBackground ? UIColor.Black : UIColor.White;
    //  videoButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
    //  videoButton.ImageEdgeInsets = new UIEdgeInsets( 5, 10, 5, 110f - 42 );
    //  videoButton.TitleEdgeInsets = new UIEdgeInsets( 0, iOSSoftwareVersion.iOS13 ? 15 : -80, 0, 0 );
    //  videoButton.Layer.BorderWidth = 2f;
    //  videoButton.Layer.BorderColor = darkBackground ? UIColor.Gray.CGColor : UIColor.Black.CGColor;

    //  videoButton.SetTitle( ResourceLib.Brands.Instance.GetString( "1227" ), UIControlState.Normal );
    //  videoButton.ClipsToBounds = true;
    //  using( var img = ResourceLib.Brands.Instance.GetImage( "Play", ResourceLib.ModuleType.Centration ) )
    //  {
    //    if( img != null )
    //    {
    //      videoButton.SetImage( img.ToUIImage(), UIControlState.Normal );
    //    }
    //  }

    //  return videoButton;
    //}

    //public static UIButton CreateVideoButton( PopupButtonVM popupButtonVM, bool darkBackground = false, SizeF popupSize = default, PointF popupPosition = default, UIPopoverArrowDirection arrowDirection = UIPopoverArrowDirection.Unknown )
    //{
    //  var videoButton = new PopupButton( popupButtonVM, ResourceLib.Brands.Instance.GetString( "1227" ), popupPosition: popupPosition, popupSize: popupSize, arrowDirection: arrowDirection );
    //  videoButton.SetTitleColor( darkBackground ? UIColor.White : UIColor.Black, UIControlState.Normal );
    //  videoButton.BackgroundColor = darkBackground ? UIColor.Black : UIColor.White;
    //  videoButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
    //  videoButton.ImageEdgeInsets = new UIEdgeInsets( 5, 10, 5, 110f - 42 );
    //  videoButton.TitleEdgeInsets = new UIEdgeInsets( 0, iOSSoftwareVersion.iOS13 ? 15 : -80, 0, 0 );
    //  videoButton.Layer.BorderWidth = 2f;
    //  videoButton.Layer.BorderColor = darkBackground ? UIColor.Gray.CGColor : UIColor.Black.CGColor;
    //  videoButton.ClipsToBounds = true;
    //  videoButton.TitleLabel.Font = UIFont.SystemFontOfSize( 18f );
    //  using( var img = ResourceLib.Brands.Instance.GetImage( "Play", ResourceLib.ModuleType.Centration ) )
    //  {
    //    if( img != null )
    //    {
    //      videoButton.SetImage( img.ToUIImage(), UIControlState.Normal );
    //    }
    //  }

    //  return videoButton;

    //}

    //public static WKWebView CreateTrainingVideoView( string url, bool fullscreen, Action closed = null )
    //{
    //  var videoWebView = new WKWebView( CGRect.Empty, new WKWebViewConfiguration()
    //  {
    //    AllowsInlineMediaPlayback = true,
    //    MediaPlaybackRequiresUserAction = false
    //  } );
    //  videoWebView.LoadRequest( new NSUrlRequest( NSUrl.FromString( url ) ) );

    //  var closeHelp = OptikamGlassButton.CreateRed( ResourceLib.Brands.Instance.GetString( "1005" ) );
    //  closeHelp.Font = UIFont.FromName( "Helvetica", 12f );
    //  closeHelp.TouchUpInside += ( s2, e2 ) =>
    //  {
    //    if( videoWebView != null )
    //    {
    //      videoWebView.RemoveFromSuperview();
    //      videoWebView.Dispose();
    //      videoWebView = null;
    //    }
    //    if( closeHelp != null )
    //    {
    //      closeHelp.RemoveFromSuperview();
    //      closeHelp.Dispose();
    //      closeHelp = null;
    //    }

    //    closed?.Invoke();
    //  };
    //  videoWebView.Add( closeHelp );
    //  closeHelp.AlignTop( videoWebView.TopAnchor, 10 + ( fullscreen ? (float)ViewHelper.SafeAreaInsets.Top : 0 ) );
    //  closeHelp.AlignRight( videoWebView.RightAnchor, -15 );
    //  closeHelp.SetSize( 100, 40 );

    //  return videoWebView;
    //}

    public static UIImage ToUIImage( this UIView view )
    {
      var renderer = new UIGraphicsImageRenderer( bounds: view.Bounds, new UIGraphicsImageRendererFormat() { } );
      return renderer.CreateImage( ( context ) =>
      {
        view.Layer.RenderInContext( context.CGContext );
      } );
    }

    public static UIImage RotateImage( this UIImage image, float degree )
    {
      //Code taken from https://stackoverflow.com/a/49689059
      float Radians = degree * (float)Math.PI / 180;

      UIView view = new UIView( frame: new CGRect( 0, 0, image.Size.Width, image.Size.Height ) );
      CGAffineTransform t = CGAffineTransform.MakeRotation( Radians );
      view.Transform = t;
      CGSize size = view.Frame.Size;

      UIGraphics.BeginImageContext( size );
      CGContext context = UIGraphics.GetCurrentContext();

      context.TranslateCTM( size.Width / 2, size.Height / 2 );
      context.RotateCTM( Radians );
      context.ScaleCTM( 1, -1 );

      context.DrawImage( new CGRect( -image.Size.Width / 2, -image.Size.Height / 2, image.Size.Width, image.Size.Height ), image.CGImage );

      UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
      UIGraphics.EndImageContext();

      return imageCopy;
    }

    public static void DisposeView( UIView view )
    {
      if( view != null )
      {
        //if( view is UIStackView )
        //{
        //  var stackView = view as UIStackView;

        //  foreach( var subView in stackView.ArrangedSubviews )
        //  {
        //    stackView.RemoveArrangedSubview( subView );
        //    DisposeView( subView );
        //  }
        //}

        view.RemoveFromSuperview();
        view.Dispose();
      }
    }

    public static void DisposeObject( IDisposable obj )
    {
      if( obj != null )
      {
        obj.Dispose();
      }
    }

    public static UIViewController GetTopViewController()
    {
      var window = UIApplication.SharedApplication.KeyWindow;
      var vc = window.RootViewController;
      while( vc.PresentedViewController != null )
        vc = vc.PresentedViewController;

      if( vc is UINavigationController navController )
        vc = navController.ViewControllers.Last();

      return vc;
    }

    public static T GetSuperView<T>( this UIView view ) where T : class
    {
      var superView = view.Superview;

      if( superView is T )
      {
        return superView as T;
      }
      else if( superView.Superview != null )
      {
        return superView.GetSuperView<T>();
      }

      return null;
    }
  }

  public static class Converters
  {
    public static Func<bool, bool> BoolToInverseBool = ( val ) => { return !val; };
  }

  //public static class BindingsExtension
  //{
  //  public static void Detach( this List<Binding> bindings )
  //  {
  //    if( bindings != null )
  //    {
  //      foreach( var binding in bindings )
  //      {
  //        binding.Detach();
  //      }
  //    }
  //  }
  //}
}

