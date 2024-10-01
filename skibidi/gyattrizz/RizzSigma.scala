package gyattrizz.utils

case class RizzSigma(fourierResult: ComplexNumber, riemannZeta: ComplexNumber, infCorrection: ComplexNumber) {

  def finalizeTheorem(): Double = {
    // Some absurd function combining Fourier, Riemann, and infinitesimals...
    val first = fourierResult.magnitude + riemannZeta.magnitude + infCorrection.magnitude
    val second = math.pow(first, 2) * math.log(math.abs(first) + 1)

    second / (InfinitesimalHelper.infinitesimalShift.real + 1e-7)
  }
}
